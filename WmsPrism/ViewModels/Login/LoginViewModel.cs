using AutoMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WmsPrism.Extensions.Encrypt;
using WmsPrism.IServices;
using WmsPrism.Model.Dto;
using WmsPrism.Services;
using WmsPrism.Views;
using static WmsPrism.EventAggregatorRepository;

namespace WmsPrism.ViewModels.Login
{
    public class LoginViewModel: BindableBase
    {
        IMapper mapper = null;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public LoginViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            ExitCommand = new DelegateCommand(Exit);
            LoginCommand = new DelegateCommand<PasswordBox>(Login);

            //MapperConfiguration mapperConfiguration = AutoMapperConfig.RegisterMappings();
            //mapper = mapperConfiguration.CreateMapper();

        }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand<PasswordBox> LoginCommand { get; private set;}



        private bool _isCancel = true;
        /// <summary>
        /// 禁用按钮
        /// </summary>
        public bool IsCancel
        {
            get { return _isCancel; }
            set { _isCancel = value; RaisePropertyChanged(); }
        }

        private bool _dialogIsOpen;
        public bool DialogIsOpen
        {
            get { return _dialogIsOpen; }
            set { _dialogIsOpen = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 登陆信息
        /// </summary>
        private string _loginMsg;
        public string LoginMsg 
        {
            get { return _loginMsg; }
            set { _loginMsg = value; RaisePropertyChanged(); }

        }

        /// <summary>
        /// 登陆用户
        /// </summary>
        private string _loginName;
        public string LoginName 
        {
            get { return _loginName; }
            set { _loginName = value; RaisePropertyChanged(); }

        }

        private void Exit()
        {
            ShellSwitcher.CloseLogin<WmsPrism.Views.Login.Login>();
        }
        private readonly static NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        private async void Login(PasswordBox passwordBox)
        {
            LoginName = "Admin";
            passwordBox.Password = "123456";
            try
            {
                string password = passwordBox.Password;
                if (string.IsNullOrEmpty(LoginName) || string.IsNullOrEmpty(password))
                {
                    LoginMsg = "请填写用户名和密码.";
                    return;
                }

                IUserServices userservices = new UserServices();

                DialogIsOpen = true;
                IsCancel = false;

                string encryptpwd = EncryptTokenHelp.EncryptTokenForPassword(password);

                UserDto userdto = await userservices.UserSign(LoginName, encryptpwd);
                if (userdto != null)
                {
                    //发布事件 用户登陆信息
                    SetPublish(userdto);
                    ShellSwitcher.LoginOpenMain<WmsPrism.Views.Login.Login>();
                }
                else
                {
                    LoginMsg = "填写信息有误,请确认后再登陆。";
                }

            }
            catch (Exception ex)
            {
                LoginMsg = "发生错误，请联系管理员";
                Logger.WriteLog("ErroLog", ex.ToString());
               
                return;
            }
            finally 
            {
                DialogIsOpen = false;
                IsCancel = true;

            }


        }


        public void SetPublish(UserDto usedto)
        {
            EventAggregatorRepository
                .GetInstance()
                .eventAggregator
                .GetEvent<LoginEvent>()
                .Publish(usedto);
        }
    }
}
