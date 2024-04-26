﻿using CsvHelper;
using CsvHelper.Configuration;
using ReactiveUI;
using SubscribersTelephoneCompany.Entities;
using SubscribersTelephoneCompany.Service;
using SubscribersTelephoneCompany.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SubscribersTelephoneCompany.ViewModels
{
    public class AbonentViewModel : ReactiveObject
    {

        private ObservableCollection<AbonentDto> _allAbonents;
        private readonly IAbonentService _abonentService;
        private ListView _lvAbonents;

        public ObservableCollection<AbonentDto> AllAbonents { get => _allAbonents; set => this.RaiseAndSetIfChanged(ref _allAbonents, value); }

        public AbonentViewModel()
        {

        }

        public AbonentViewModel(ListView lvAbonents, IAbonentService abonentService)
        {
            _lvAbonents = lvAbonents;
            _abonentService = abonentService;
            List<AbonentDto> abonents = _abonentService.GetAbonentList();

            AllAbonents = new ObservableCollection<AbonentDto>(abonents);
        }



        public ReactiveCommand<Object, Unit> ExportToCsv => ReactiveCommand.Create<object>(o =>
        {
            _abonentService.ExportToCsv(_lvAbonents, AllAbonents);
        });

        public ReactiveCommand<Object, Unit> OpenStreetWindow => ReactiveCommand.Create<object>(o =>
        { 
            List<StreetDto> streets = new List<StreetDto>();
            streets = _abonentService.GetStreetAbonent();

            var streetWindow = new StreetsServedWindow(streets);
            streetWindow.Show();

        });
    }
}




    

