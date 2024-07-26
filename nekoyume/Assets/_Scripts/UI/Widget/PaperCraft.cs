﻿using System;
using System.Linq;
using Nekoyume.Blockchain;
using Nekoyume.Game;
using Nekoyume.Model.Item;
using Nekoyume.Model.Mail;
using Nekoyume.State;
using Nekoyume.UI.Model;
using Nekoyume.UI.Scroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nekoyume.UI
{
    using UniRx;
    public class PaperCraft : Widget
    {
        [Serializable]
        private class SubTypeButton
        {
            public ItemSubType itemSubType;
            public Button button;
        }

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private SubTypeButton[] subTypeButtons;

        // [SerializeField]
        // private ConditionalCostButton conditionalCostButton;

        [SerializeField]
        private Button craftButton;

        [SerializeField]
        private Button skillHelpButton;

        [SerializeField]
        private Button skillListButton;

        [SerializeField]
        private TextMeshProUGUI subTypePaperText;

        [SerializeField]
        private TextMeshProUGUI skillText;

        [SerializeField]
        private SimpleItemScroll outfitScroll;

        private Item _selectedItem;

        private ItemSubType _selectedSubType = ItemSubType.Weapon;

        protected override void Awake()
        {
            base.Awake();
            closeButton.onClick.AddListener(() =>
            {
                Close(true);
                Find<CombinationMain>().Show();
            });
            skillHelpButton.onClick.AddListener(() =>
            {
                NcDebug.Log("skillHelpButton onclick");
                // 뭐시기팝업.Show();
            });
            skillListButton.onClick.AddListener(() =>
            {
                Find<SummonSkillsPopup>().Show(TableSheets.Instance.SummonSheet.First);
                NcDebug.Log("skillListButton onclick");
            });
            foreach (var subTypeButton in subTypeButtons)
            {
                subTypeButton.button.onClick.AddListener(() =>
                {
                    OnItemSubtypeSelected(subTypeButton.itemSubType);
                });
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            ReactiveAvatarState.ObservableRelationship
                .Where(_ => isActiveAndEnabled)
                .Subscribe(SetSkillView)
                .AddTo(gameObject);

            outfitScroll.OnClick.Subscribe(item =>
            {
                if (_selectedItem != null)
                {
                    _selectedItem.Selected.Value = false;
                }

                _selectedItem = item;
                _selectedItem.Selected.Value = true;
            }).AddTo(gameObject);
            craftButton.onClick.AddListener(OnClickSubmitButton);
        }

        public override void Show(bool ignoreShowAnimation = false)
        {
            base.Show(ignoreShowAnimation);
            _selectedItem = null;
            SetSkillView(ReactiveAvatarState.Relationship);
            OnItemSubtypeSelected(ItemSubType.Weapon);
        }

        /// <summary>
        /// 숙련도의 상태를 표시하는 View update 코드이다.
        /// State를 보여주는 기능으로, ActionRenderHandler나 ReactiveAvatarState를 반영해야 한다.
        /// </summary>
        /// <param name="skill"></param>
        private void SetSkillView(long skill)
        {
            skillText.SetText($"SKILL: {skill}");
        }

        /// <summary>
        /// 어떤 종류의 장비를 만들지 ItemSubType을 선택하면 실행될 콜백, View 업데이트를 한다
        /// </summary>
        /// <param name="type"></param>
        private void OnItemSubtypeSelected(ItemSubType type)
        {
            // TODO: 지금은 그냥 EquipmentItemRecipeSheet에서 해당하는 ItemSubType을 다 뿌리고있다.
            // 나중에 외형을 갖고있는 시트 데이터로 바꿔치기 해야한다.
            _selectedSubType = type;
            subTypePaperText.SetText($"{_selectedSubType} PAPER");
            outfitScroll.UpdateData(TableSheets.Instance.CustomEquipmentCraftIconSheet.Values
                .Where(row => row.ItemSubType == _selectedSubType).Select(r =>
                    new Item(ItemFactory.CreateItem(Game.Game.instance.TableSheets
                        .EquipmentItemSheet[r.IconId], new ActionRenderHandler.LocalRandom(0)))));
        }

        private void OnClickSubmitButton()
        {
            if (_selectedItem is null)
            {
                OneLineSystem.Push(MailType.System, "select outfit", NotificationCell.NotificationType.Information);
                return;
            }

            if (Find<CombinationSlotsPopup>().TryGetEmptyCombinationSlot(out var slotIndex))
            {
                // TODO: 전부 다 CustomEquipmentCraft 관련 sheet에서 가져오게 바꿔야함
                ActionManager.Instance.CustomEquipmentCraft(slotIndex,
                        TableSheets.Instance.CustomEquipmentCraftRecipeSheet.Values.First(r =>
                            r.ItemSubType == _selectedItem.ItemBase.Value.ItemSubType).Id, _selectedItem.ItemBase.Value.Id)
                    .Subscribe();
            }
        }

        public override void Close(bool ignoreCloseAnimation = false)
        {
            base.Close(ignoreCloseAnimation);
            _selectedItem = null;
        }
    }
}
