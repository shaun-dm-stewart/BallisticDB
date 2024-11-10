using BallisticDB.Services;
using System.ComponentModel.DataAnnotations;

namespace BallisticDB.ViewModels
{
    public class RifleViewModel : ViewModelBase
    {
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
                }
            }
        }

        private string _rifleName = string.Empty;
        public string RifleName
        {
            get { return _rifleName; }
            set
            {
                if (value != _rifleName)
                {
                    OnPropertyChanging();
                    _rifleName = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private double _scopeHeight;
        public double ScopeHeight
        {
            get { return _scopeHeight; }
            set
            {
                if (value != _scopeHeight)
                {
                    OnPropertyChanging();
                    _scopeHeight = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private double _twistRate;
        public double TwistRate
        {
            get { return _twistRate; }
            set
            {
                if (value != _twistRate)
                {
                    OnPropertyChanging();
                    _twistRate = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private double _zeroDistance;
        public double ZeroDistance
        {
            get { return _zeroDistance; }
            set
            {
                if (value != _zeroDistance)
                {
                    OnPropertyChanging();
                    _zeroDistance = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private double _elevationClicksPerMOA;
        public double ElevationClicksPerMOA
        {
            get { return _elevationClicksPerMOA; }
            set
            {
                if (value != _elevationClicksPerMOA)
                {
                    OnPropertyChanging();
                    _elevationClicksPerMOA = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private double _windageClicksPerMOA;
        public double WindageClicksPerMOA
        {
            get { return _windageClicksPerMOA; }
            set
            {
                if (value != _windageClicksPerMOA)
                {
                    OnPropertyChanging();
                    _windageClicksPerMOA = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private double _altitude;
        public double Altitude
        {
            get { return _altitude; }
            set
            {
                if (value != _altitude)
                {
                    OnPropertyChanging();
                    _altitude = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private double _atmosphericPressure;
        public double AtmosphericPressure
        {
            get { return _atmosphericPressure; }
            set
            {
                if (value != _atmosphericPressure)
                {
                    OnPropertyChanging();
                    _atmosphericPressure = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private double _temperature;
        public double Temperature
        {
            get { return _temperature; }
            set
            {
                if (value != _temperature)
                {
                    OnPropertyChanging();
                    _temperature = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private double _relativeHumidity;
        public double RelativeHumidity
        {
            get { return _relativeHumidity; }
            set
            {
                if (value != _relativeHumidity)
                {
                    OnPropertyChanging();
                    _relativeHumidity = value;
                    switch (RowState)
                    {
                        case RowStatus.UNCHANGED:
                            RowState = RowStatus.UPDATED;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public RowStatus RowState { get; set; } = RowStatus.UNCHANGED;

    }
}
