using AutoMapper;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WmsPrism.Extensions.AutoMapper;
using WmsPrism.IServices;
using WmsPrism.Model.Dto;
using WmsPrism.Model.Models;
using WmsPrism.Services;


namespace WmsPrism.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        //IMapper mapper = null;

        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public static UserDto loginUserDto;
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            //MapperConfiguration mapperConfiguration = AutoMapperConfig.RegisterMappings();
            //mapper = mapperConfiguration.CreateMapper();

            SelectedItemChangedCommand = new DelegateCommand<object>(SelectedChange);
            ClosePageCommand = new DelegateCommand<string>(ClosePage);

            OpenPageCommand = new DelegateCommand<string>(OpenPage);


            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;


            //加载Tab试试
            ModuleGroups = new ObservableCollection<GroupManager>();

            //订阅事件
            SetSubscribe();

        }



        //public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<object> SelectedItemChangedCommand { get; private set; }


        public DelegateCommand<string> ClosePageCommand { get; private set; }

        public DelegateCommand<string> OpenPageCommand { get; private set; }

        private ObservableCollection<GroupManager> moduleGroups;

        /// <summary>
        /// Tab选项卡
        /// </summary>
        public ObservableCollection<GroupManager> ModuleGroups
        {
            get { return moduleGroups; }
            set { moduleGroups = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<MovieCategory> _movieCategories;
        public ObservableCollection<MovieCategory> MovieCategories
        {
            get { return _movieCategories; }
            set { _movieCategories = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<Movie> _movies;
        public ObservableCollection<Movie> Movies
        {
            get { return _movies; }
            set { _movies = value; RaisePropertyChanged(); }
        }


        public void SetSubscribe()
        {
            EventAggregatorRepository
                .GetInstance()
                .eventAggregator
                .GetEvent<WmsPrism.EventAggregatorRepository.LoginEvent>()
                .Subscribe(LoginUserInfo, ThreadOption.UIThread, true);
        }

        public async void LoginUserInfo(UserDto userdto)
        {
            loginUserDto = userdto;
            IUserServices userservices = new UserServices();
            //加载菜单
            List<WMS_menu> menus = await userservices.GetUserMenu(userdto.Menu_ids);
            List<WMS_menu> menuNotNull = menus.Where(x => x.Module!="").ToList();

            Movie[] movies = new Movie[menuNotNull.Count];

            for (int i = 0; i < menuNotNull.Count; i++)
            {
                Movie movie = new Movie(menuNotNull[i].Title, menuNotNull[i].Module);
                movies[i] = movie;
            }

            MovieCategories = new ObservableCollection<MovieCategory>();
            //目前只有一级
            MovieCategories.Add(new MovieCategory("菜单", movies));
        }

        public void SelectedChange(object obj)
        {
            if (obj.GetType() == typeof(MovieCategory))
            {
                return;
            }
            else if(obj.GetType() == typeof(Movie))
            {
                Movie movie = (Movie)obj;
                //带个值过去清空
                NavigationParameters param = new NavigationParameters();
                param.Add("LoginUserInfo", loginUserDto);
                regionManager.RequestNavigate("ContentRegion", movie.Director, param);

                //写死
                if (movie.Name != "打印托运单")
                {
                    ModuGroupsControl(movie.Name,movie.Director);
                  
                }

            }

        }


        public void ModuGroupsControl(string name,string director) 
        {
            var groups = ModuleGroups.Where(s => s.Tite == name).FirstOrDefault();

            if (groups == null)
            {
                ModuleGroups.Add(new GroupManager { Tite = name,Url= director });
            }

        }

        //删除时候 需要优化,要一个变量保存当前打开的页面 如果关闭的页面=当前页面 把当前页面也关闭 并导航
        public void ClosePage(string pageName)
        {
            var groups = ModuleGroups.Where(s => s.Tite == pageName).FirstOrDefault();
            if (groups != null)
            {
                ModuleGroups.Remove(groups);
            }
        }

        public void OpenPage(string pageName) 
        {
            var module= ModuleGroups.Where(s => s.Tite== pageName).FirstOrDefault();

            if (module != null)
            {
                //导航
                NavigationParameters param = new NavigationParameters();
                param.Add("LoginUserInfo", loginUserDto);
                regionManager.RequestNavigate("ContentRegion", module.Url, param);
            }
        }
    }


    public sealed class MovieCategory
    {
        public MovieCategory(string name, params Movie[] movies)
        {
            Name = name;

            Movies = new ObservableCollection<Movie>(movies);
        }

        public string Name { get; }

        public ObservableCollection<Movie> Movies { get; set; }
    }

    public sealed class Movie
    {
        public Movie(string name, string director)
        {
            Name = name;
            Director = director;
        }

        public string Name { get; set; }

        public string Director { get; set; }
    }

    public class GroupManager
    {
        public string Tite { get; set; }
        public string Url { get; set; }
    }
}
