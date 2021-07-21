using Nekoyume.Model.State;
using Nekoyume.State;
using Nekoyume.UI.Model;
using Nekoyume.UI.Module;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Nekoyume.UI.Scroller
{
    public class RankCellPanel : MonoBehaviour
    {
        [SerializeField]
        private Image rankImage = null;

        [SerializeField]
        private TextMeshProUGUI rankText = null;

        [SerializeField]
        private DetailedCharacterView characterView = null;

        [SerializeField]
        private TextMeshProUGUI nicknameText = null;

        [SerializeField]
        private TextMeshProUGUI addressText = null;

        [SerializeField]
        private TextMeshProUGUI firstElementCpText = null;

        [SerializeField]
        private TextMeshProUGUI firstElementText = null;

        [SerializeField]
        private TextMeshProUGUI secondElementText = null;

        [SerializeField]
        private TextMeshProUGUI secondElementEquipmentNameText = null;

        [SerializeField]
        private Sprite firstPlaceSprite = null;

        [SerializeField]
        private Sprite secondPlaceSprite = null;

        [SerializeField]
        private Sprite thirdPlaceSprite = null;

        [SerializeField]
        private int addressStringCount = 6;

        private void Awake()
        {
            characterView.OnClickCharacterIcon
                .Subscribe(avatarState =>
                {
                    if (avatarState is null)
                    {
                        return;
                    }

                    Widget.Find<FriendInfoPopup>().Show(avatarState);
                })
                .AddTo(gameObject);
        }

        public void SetDataAsAbility(AbilityRankingModel rankingInfo)
        {
            nicknameText.text = rankingInfo.AvatarState.name;
            nicknameText.gameObject.SetActive(true);
            addressText.text = rankingInfo.AvatarState.address
                .ToString()
                .Remove(addressStringCount);

            firstElementCpText.text = rankingInfo.Cp.ToString();
            secondElementText.text = rankingInfo.AvatarState.level.ToString();

            firstElementText.gameObject.SetActive(false);
            firstElementCpText.gameObject.SetActive(true);
            secondElementText.gameObject.SetActive(true);
            secondElementEquipmentNameText.gameObject.SetActive(false);

            UpdateRank(rankingInfo.Rank);
            characterView.SetByAvatarState(rankingInfo.AvatarState);
            gameObject.SetActive(true);
        }

        public void SetDataAsStage(StageRankingModel rankingInfo)
        {
            nicknameText.text = rankingInfo.AvatarState.name;
            nicknameText.gameObject.SetActive(true);
            addressText.text = rankingInfo.AvatarState.address
                .ToString()
                .Remove(addressStringCount);

            firstElementText.text = rankingInfo.ClearedStageId.ToString();
            firstElementText.gameObject.SetActive(true);
            firstElementCpText.gameObject.SetActive(false);
            secondElementText.gameObject.SetActive(false);
            secondElementEquipmentNameText.gameObject.SetActive(false);

            UpdateRank(rankingInfo.Rank);
            characterView.SetByAvatarState(rankingInfo.AvatarState);
            gameObject.SetActive(true);
        }

        public void SetDataAsCrafting(CraftRankingModel rankingInfo)
        {
            nicknameText.text = rankingInfo.AvatarState.name;
            nicknameText.gameObject.SetActive(true);
            addressText.text = rankingInfo.AvatarState.address
                .ToString()
                .Remove(addressStringCount);

            firstElementText.text = rankingInfo.CraftCount.ToString();
            firstElementText.gameObject.SetActive(true);
            firstElementCpText.gameObject.SetActive(false);
            secondElementText.gameObject.SetActive(false);
            secondElementEquipmentNameText.gameObject.SetActive(false);

            UpdateRank(rankingInfo.Rank);
            characterView.SetByAvatarState(rankingInfo.AvatarState);
            gameObject.SetActive(true);
        }

        public void SetDataAsEquipment(EquipmentRankingModel rankingInfo)
        {
            nicknameText.text = rankingInfo.AvatarState.name;
            nicknameText.gameObject.SetActive(true);
            addressText.text = rankingInfo.AvatarState.address
                .ToString()
                .Remove(addressStringCount);

            firstElementCpText.text = rankingInfo.Cp.ToString();

            var equipmentItemSheet = Game.Game.instance.TableSheets.EquipmentItemSheet;
            secondElementEquipmentNameText.text = LocalizationExtension.GetLocalizedName(
                equipmentItemSheet,
                rankingInfo.EquipmentId,
                rankingInfo.Level);

            firstElementText.gameObject.SetActive(false);
            firstElementCpText.gameObject.SetActive(true);
            secondElementText.gameObject.SetActive(false);
            secondElementEquipmentNameText.gameObject.SetActive(true);

            UpdateRank(rankingInfo.Rank);
            characterView.SetByAvatarState(rankingInfo.AvatarState);
            gameObject.SetActive(true);
        }

        private void UpdateRank(int rank)
        {
            switch (rank)
            {
                case 0:
                    rankImage.gameObject.SetActive(false);
                    rankText.gameObject.SetActive(true);
                    rankText.text = "-";
                    break;
                case 1:
                    rankImage.gameObject.SetActive(true);
                    rankText.gameObject.SetActive(false);
                    rankImage.sprite = firstPlaceSprite;
                    break;
                case 2:
                    rankImage.gameObject.SetActive(true);
                    rankText.gameObject.SetActive(false);
                    rankImage.sprite = secondPlaceSprite;
                    break;
                case 3:
                    rankImage.gameObject.SetActive(true);
                    rankText.gameObject.SetActive(false);
                    rankImage.sprite = thirdPlaceSprite;
                    break;
                default:
                    rankImage.gameObject.SetActive(false);
                    rankText.gameObject.SetActive(true);
                    rankText.text = rank.ToString();
                    break;
            }
        }

        public void SetEmpty(AvatarState avatarState)
        {
            rankText.text = "-";
            characterView.SetByAvatarState(avatarState);
            nicknameText.text = avatarState.name;
            addressText.text = avatarState.address.ToString()
                .Remove(addressStringCount);

            firstElementText.text = "-";
            firstElementText.gameObject.SetActive(true);
            firstElementCpText.gameObject.SetActive(false);
            secondElementText.gameObject.SetActive(false);
            secondElementEquipmentNameText.gameObject.SetActive(false);
        }
    }
}
