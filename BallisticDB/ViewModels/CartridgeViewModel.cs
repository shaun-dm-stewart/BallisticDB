﻿using BallisticDB.Messages;
using BallisticDB.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace BallisticDB.ViewModels
{
    public class CartridgeViewModel : ViewModelBase
    {
        public CartridgeViewModel()
        {
        }

        public CartridgeViewModel(long rifleId
            , long cartridgeId
            , string cartridgeName
            , double weight
            , double muzzleVelocity
            , double ballisticCoefficient
            , double bulletLength
            , double calibre
        )
        {
            _rifleId = rifleId;
            _cartridgeId = cartridgeId;
            _cartridgeName = cartridgeName;
            _weight = weight;
            _muzzleVelocity = muzzleVelocity;
            _ballisticCoefficient = ballisticCoefficient;
            _bulletLength = bulletLength;
            _calibre = calibre;
        }

        private long _rifleId;
        public long RifleId
        {
            get { return _rifleId; }
            set
            {
                if (value != _rifleId)
                {
                    OnPropertyChanging();
                    _rifleId = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                    SendDataChangedMessage("Cartridge rifle Id updated");
                }
            }
        }

        private long _cartridgeId;
        public long CartridgeId
        {
            get { return _cartridgeId; }
            set
            {
                if (value != _cartridgeId)
                {
                    OnPropertyChanging();
                    _cartridgeId = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                    SendDataChangedMessage("Cartridge Id updated");
                }
            }
        }

        private string _cartridgeName = string.Empty;
        public string CartridgeName
        {
            get { return _cartridgeName; }
            set
            {
                if (value != _cartridgeName)
                {
                    OnPropertyChanging();
                    _cartridgeName = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                    SendDataChangedMessage("Cartridge name updated");
                }
            }
        }

        private double _weight;
        public double Weight
        {
            get { return _weight; }
            set
            {
                if (value != _weight)
                {
                    OnPropertyChanging();
                    _weight = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                    SendDataChangedMessage("Cartridge weight updated");
                }
            }
        }

        private double _muzzleVelocity;
        public double MuzzleVelocity
        {
            get { return _muzzleVelocity; }
            set
            {
                if (value != _muzzleVelocity)
                {
                    OnPropertyChanging();
                    _muzzleVelocity = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                    SendDataChangedMessage("Cartridge mv updated");
                }
            }
        }

        private double _ballisticCoefficient;
        public double BallisticCoefficient
        {
            get { return _ballisticCoefficient; }
            set
            {
                if (value != _ballisticCoefficient)
                {
                    OnPropertyChanging();
                    _ballisticCoefficient = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                    SendDataChangedMessage("Cartridge bc updated");
                }
            }
        }

        private double _bulletLength;
        public double BulletLength
        {
            get { return _bulletLength; }
            set
            {
                if (value != _bulletLength)
                {
                    OnPropertyChanging();
                    _bulletLength = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                    SendDataChangedMessage("Cartridge bc updated");
                }
            }
        }

        private double _calibre;
        public double Calibre
        {
            get { return _calibre; }
            set
            {
                if (value != _calibre)
                {
                    OnPropertyChanging();
                    _calibre = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                    SendDataChangedMessage("Cartridge calibre updated");
                }
            }
        }

        public RowStatus RowState { get; set; } = RowStatus.UNCHANGED;

        private void SendDataChangedMessage(string msg)
        {
            WeakReferenceMessenger.Default.Send(new DataChangedMessage(new DataStatus(true, msg)));
        }
    }
}
