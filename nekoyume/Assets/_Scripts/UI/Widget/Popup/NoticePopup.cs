﻿using System;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine;
using Nekoyume.Game.Controller;

namespace Nekoyume.UI
{
    public class NoticePopup : PopupWidget
    {
        [SerializeField] private Blur blur;
        [SerializeField] private Button detailButton;
        [SerializeField] private Button closeButton;
        
        private const string NoticeBeginTime = "2022/03/01 00:00:00";
        private const string NoticeEndTime = "2022/03/31 00:00:00";
        private const string NoticePageUrlFormat = "https://www.notion.so/planetarium/1bc6de399b3b4ace95fca3a3020b4d79";
        private const string LastNoticeDayKey = "NOTICE_POPUP_LAST_DAY";

        private static bool CanShowNoticePopup
        {
            get
            {
                var now = DateTime.UtcNow;
                var begin = DateTime.Parse(NoticeBeginTime);
                var end = DateTime.Parse(NoticeEndTime);
                var isInTime = now >= begin && now <= end;
                
                if(!isInTime) return false;
                
                var lastNoticeData = PlayerPrefs.GetString(LastNoticeDayKey, "2022/03/01 00:00:00");
                var lastNoticeDay = DateTime.Parse(lastNoticeData);
                var isNewDay = now.Year != lastNoticeDay.Year || now.Month != lastNoticeDay.Month || now.Day != lastNoticeDay.Day;
                if (isNewDay)
                {
                    PlayerPrefs.SetString(LastNoticeDayKey, now.ToString(CultureInfo.InvariantCulture));
                }

                return isNewDay;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            detailButton.onClick.AddListener(() =>
            {
                GoToNoticePage();
                AudioController.PlayClick();
            });
            
            closeButton.onClick.AddListener(() =>
            {
                Close();
                AudioController.PlayClick();
            });

            blur.button.onClick.AddListener(() => Close(true));
        }

        public override void Show(bool ignoreStartAnimation = false)
        {
            if(!CanShowNoticePopup) return;
            base.Show(ignoreStartAnimation);

            if (blur)
            {
                blur.Show();
            }
            // HelpTooltip.HelpMe(100014, true);
        }
        
        public override void Close(bool ignoreCloseAnimation = false)
        {
            if (blur && blur.isActiveAndEnabled)
            {
                blur.Close();
            }

            base.Close(ignoreCloseAnimation);
        }

        private static void GoToNoticePage()
        {
            Application.OpenURL(NoticePageUrlFormat);
        }
    }
}
