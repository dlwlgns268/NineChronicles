using System;
using System.Collections;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Libplanet.Crypto;
using Nekoyume.Planet;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Nekoyume.UI
{
    public class PortalConnect
    {
        [Serializable]
        public class RequestResult
        {
            public string title;
            public string message;
            public string resultCode;
        }

        [Serializable]
        public class RequestCodeResult : RequestResult
        {
            public string code;
        }

        [Serializable]
        public class AccessTokenResult : RequestResult
        {
            public string accessToken;
            public string refreshToken;
            public string address;
        }

        [Serializable]
        public class RequestPledgeResult : RequestResult
        {
            public string txId;
        }

        private System.Action _onPortalEnd;
        private string deeplinkURL;

        private string clientSecret;
        private string code;
        private string accessToken;
        private string txId;

        public readonly string PortalUrl;
        public const string GoogleAuthEndpoint = "/api/auth/login/google";
        private const string RequestCodeEndpoint = "/api/auth/code";
        private const string RequestPledgeEndpoint = "/api/account/mobile/contract";
        private const string AccessTokenEndpoint = "/api/auth/token";
        private const string PortalRewardEndpoint = "/earn#Play";
        private const string ClientSecretKey = "Cached_ClientSecret";
        private const int Timeout = 180;

        public PortalConnect(string url)
        {
            PortalUrl = url ?? "https://nine-chronicles.com";

            Application.deepLinkActivated += OnDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnDeepLinkActivated(Application.absoluteURL);
            }
            else deeplinkURL = "[none]";
        }

        public void OpenPortal(System.Action onPortalEnd = null)
        {
            _onPortalEnd = onPortalEnd;

            clientSecret = GetClientSecret();
            Application.OpenURL($"{PortalUrl}/mobile-signin?clientSecret={clientSecret}");
            Analyzer.Instance.Track("Unity/Portal/1");
        }

        private void OnDeepLinkActivated(string url)
        {
            deeplinkURL = url;

            if (_onPortalEnd != null)
            {
                _onPortalEnd();
                _onPortalEnd = null;
            }

            if (Widget.Find<LoginSystem>().KeyStore.ListIds().Any())
            {
                return;
            }

            Analyzer.Instance.Track("Unity/Portal/2");

            var param = deeplinkURL.Split('?')[1].Split('&')
                .ToDictionary(str => str.Split('=')[0], str => str.Split('=')[1]);

            if (param.TryGetValue("clientSecret", out var outClientSecret))
            {
                clientSecret = outClientSecret;
            }

            var accountExist = param.ContainsKey("ncAddress");
            if (param.TryGetValue("code", out var outCode))
            {
                code = outCode;
                if (string.IsNullOrEmpty(code) && !accountExist)
                {
                    RequestCode(OnSuccessAsync);
                    return;
                }
            }

            OnSuccessAsync().Forget();
            return;

            async UniTaskVoid OnSuccessAsync()
            {
                if (!accountExist)
                {
                    await AccessTokenAsync();
                }

                Address? address = accountExist
                    ? new Address(param["ncAddress"])
                    : null;
                Widget.Find<LoginSystem>().Show(address);
            }
        }

        private static string GetClientSecret()
        {
            if (PlayerPrefs.HasKey(ClientSecretKey))
            {
                return PlayerPrefs.GetString(ClientSecretKey);
            }

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const int length = 16;
            var stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[Random.Range(0, chars.Length)]);
            }

            var clientSecret = stringBuilder.ToString();
            PlayerPrefs.SetString(ClientSecretKey, clientSecret);
            PlayerPrefs.Save();

            return clientSecret;
        }

        private async void RequestCode(Func<UniTaskVoid> onSuccessAsync)
        {
            Analyzer.Instance.Track("Unity/Portal/3");

            var url = $"{PortalUrl}{RequestCodeEndpoint}?clientSecret={clientSecret}";
            var form = new WWWForm();
            var request = UnityWebRequest.Post(url, form);
            request.timeout = Timeout;

            await request.SendWebRequest();

            var json = request.downloadHandler.text;
            var data = JsonUtility.FromJson<RequestCodeResult>(json);
            if (request.result == UnityWebRequest.Result.Success)
            {
                if (!string.IsNullOrEmpty(data.code))
                {
                    code = data.code;
                    onSuccessAsync.Invoke();
                }
                else
                {
                    Debug.LogError($"AccessToken Deserialize Error: {json}");
                    ShowRequestErrorPopup(data);
                }
            }
            else
            {
                Debug.LogError($"AccessToken Error: {request.error}\n{json}\nclientSecret: {clientSecret}");
                ShowRequestErrorPopup(request.result, request.error);
            }
        }

        private async UniTask AccessTokenAsync()
        {
            var url = $"{PortalUrl}{AccessTokenEndpoint}";
            Debug.Log($"[PortalConnect] AccessTokenAsync url: {url}");

            var form = new WWWForm();
            form.AddField("clientSecret", clientSecret);
            form.AddField("code", code);

            var request = UnityWebRequest.Post(url, form);
            request.timeout = Timeout;

            try
            {
                await request.SendWebRequest();
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogException(e);
            }

            HandleAccessTokenResult(request);
        }

        public bool HandleAccessTokenResult(UnityWebRequest request)
        {
            var json = request.downloadHandler.text;
            var data = JsonUtility.FromJson<AccessTokenResult>(json);
            if (request.result == UnityWebRequest.Result.Success)
            {
                if (!string.IsNullOrEmpty(data.accessToken))
                {
                    accessToken = data.accessToken;
                    Debug.Log($"[PortalConnect] accessToken: {accessToken}");
                    return true;
                }

                Debug.LogError($"AccessToken Deserialize Error: {json}");
                ShowRequestErrorPopup(data);
                return false;
            }

            Debug.LogError($"AccessToken Error: {request.error}\n{json}\ncode: {code}\nclientSecret: {clientSecret}");
            ShowRequestErrorPopup(request.result, request.error);
            return false;
        }

        public void OpenPortalRewardUrl()
        {
            Application.OpenURL($"{PortalUrl}{PortalRewardEndpoint}");
        }

        public IEnumerator RequestPledge(PlanetId planetId, Address address)
        {
            var url = $"{PortalUrl}{RequestPledgeEndpoint}";
            var os = string.Empty;
#if UNITY_ANDROID
            os = "android";
#elif UNITY_IOS
            os = "ios";
#endif

            var form = new WWWForm();
            form.AddField("address", address.ToHex());
            form.AddField("os", os);
            form.AddField("planetId", planetId.ToString());

            var request = UnityWebRequest.Post(url, form);
            request.timeout = Timeout;
            request.SetRequestHeader("authorization", $"Bearer {accessToken}");

            yield return request.SendWebRequest();

            var json = request.downloadHandler.text;
            var data = JsonUtility.FromJson<RequestPledgeResult>(json);
            if (request.result == UnityWebRequest.Result.Success)
            {
                if (!string.IsNullOrEmpty(data.txId))
                {
                    txId = data.txId;
                    PlayerPrefs.DeleteKey(ClientSecretKey);
                }
                else
                {
                    Debug.LogError($"RequestPledge Deserialize Error: {json}");
                    ShowRequestErrorPopup(data);
                }
            }
            else
            {
                Debug.LogError($"RequestPledge Error: {request.error}\n{json}\naddress: {address.ToHex()}\nos: {os}");
                ShowRequestErrorPopup(request.result, request.error);
            }
        }

        private static void ShowRequestErrorPopup(RequestResult data)
        {
            var message = "An abnormal condition has been identified. Please try again after finishing the app.";
            message += string.IsNullOrEmpty(data.message) ? string.Empty : $"\n{data.message}";
            message += string.IsNullOrEmpty(data.resultCode) ? string.Empty : $"\nResponse code : {data.resultCode}";

            var popup = Widget.Find<TitleOneButtonSystem>();
            popup.Show(data.title, message, "OK", false);
            popup.SubmitCallback = Application.Quit;
            Analyzer.Instance.Track("Unity/Portal/0");
        }

        private static void ShowRequestErrorPopup(UnityWebRequest.Result result, string errorMessage)
        {
            var message = "An abnormal condition has been identified. Please try again after finishing the app.";
            message += string.IsNullOrEmpty(errorMessage) ? string.Empty : $"\n{errorMessage}";

            var popup = Widget.Find<TitleOneButtonSystem>();
            popup.Show(result.ToString(), message, "OK", false);
            popup.SubmitCallback = Application.Quit;
            Analyzer.Instance.Track("Unity/Portal/0");
        }
    }
}
