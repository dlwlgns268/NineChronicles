using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Nekoyume.UI
{
    using Helper;
    using UniRx;

    public class AdventureBossFullBountyStatusPopup : PopupWidget
    {
        [SerializeField] private BountyViewScroll scrollView;
        [SerializeField] private BountyCell myBountyCell;
        [SerializeField] private UnityEngine.UI.Extensions.Scroller scroller;

        private readonly List<IDisposable> _disposables = new();

        public override void Show(bool ignoreShowAnimation = false)
        {
            base.Show(ignoreShowAnimation);
            scroller.Position = 0;
            Game.Game.instance.AdventureBossData.BountyBoard.Subscribe(bountyBoard =>
            {
                if (bountyBoard == null)
                {
                    scrollView.ClearData();
                    return;
                }

                // Update Default my bounty cell
                myBountyCell.UpdateContent(new BountyItemData {
                    Rank = -1,
                    Name = Game.Game.instance.States.CurrentAvatarState.name,
                    Count = 0,
                    Ncg = 0,
                    Bonus = 0,
                });

                scrollView.UpdateData(bountyBoard.Investors.OrderByDescending(investor => investor.Price).Select((x, i) =>
                {
                    var data = new BountyItemData
                    {
                        Rank = i + 1,
                        Name = x.Name,
                        Count = x.Count,
                        Ncg = x.Price.MajorUnit,
                        Bonus = i == 0 ? (float)AdventureBossHelper.TotalRewardMultiplier : 0
                    };

                    if (x.AvatarAddress == Game.Game.instance.States.CurrentAvatarState.address)
                    {
                        myBountyCell.UpdateContent(data);
                    }

                    return data;
                }));
            }).AddTo(_disposables);
        }

        public override void Close(bool ignoreCloseAnimation = false)
        {
            base.Close(ignoreCloseAnimation);
            _disposables.DisposeAllAndClear();
        }
    }
}
