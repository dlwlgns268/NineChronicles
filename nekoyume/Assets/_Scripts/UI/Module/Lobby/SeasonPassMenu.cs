using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Nekoyume.ApiClient;

namespace Nekoyume.UI.Module.Lobby
{
    using Nekoyume.L10n;
    using UniRx;

    public class SeasonPassMenu : MainMenu
    {
        [SerializeField]
        private GameObject premiumIcon;

        [SerializeField]
        private GameObject premiumPlusIcon;

        [SerializeField]
        private TextMeshProUGUI levelText;

        [SerializeField]
        private TextMeshProUGUI timeText;

        [SerializeField]
        private GameObject notificationObj;

        [SerializeField]
        private GameObject dim;

        [SerializeField]
        private GameObject iconRoot;

        protected override void Awake()
        {
            base.Awake();

            dim.SetActive(false);
            iconRoot.SetActive(true);

            premiumIcon.SetActive(false);
            premiumPlusIcon.SetActive(false);

            int claimCount = ApiClients.Instance.SeasonPassServiceManager.HasClaimPassType.Count + ApiClients.Instance.SeasonPassServiceManager.HasPrevClaimPassType.Count;
            notificationObj.SetActive(claimCount > 0);
            levelText.text = L10nManager.Localize("SEASON_PASS_MENU_NAME");
            ApiClients.Instance.SeasonPassServiceManager.RemainingDateTime.Subscribe((endDate) => { timeText.text = $"<Style=Clock> {endDate}"; });
        }
    }
}
