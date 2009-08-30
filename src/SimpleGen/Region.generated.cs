// <autogenerated>
//   This file was generated by T4 code generator DataClasses1.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

namespace Simple.Generator
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    [Table(Name = "dbo.Region")]
    public partial class Region : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int regionID;
        private string regionDescription;
        private EntitySet<Territory> territories;
        
        public Region()
        {
            this.territories = new EntitySet<Territory>(this.AttachTerritories, this.DetachTerritories);
            this.OnCreated();
        }
        
        public event PropertyChangingEventHandler PropertyChanging;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [Column(Name = "RegionID", Storage = "regionID", CanBeNull = false, DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int RegionID
        {
            get
            {
                return this.regionID;
            }
        
            set
            {
                if (this.regionID != value)
                {
                    this.OnRegionIDChanging(value);
                    this.SendPropertyChanging("RegionID");
                    this.regionID = value;
                    this.SendPropertyChanged("RegionID");
                    this.OnRegionIDChanged();
                }
            }
        }

        [Column(Name = "RegionDescription", Storage = "regionDescription", CanBeNull = false, DbType = "NChar(50) NOT NULL")]
        public string RegionDescription
        {
            get
            {
                return this.regionDescription;
            }
        
            set
            {
                if (this.regionDescription != value)
                {
                    this.OnRegionDescriptionChanging(value);
                    this.SendPropertyChanging("RegionDescription");
                    this.regionDescription = value;
                    this.SendPropertyChanged("RegionDescription");
                    this.OnRegionDescriptionChanged();
                }
            }
        }

        [Association(Name = "Region_Territory", Storage = "territories", ThisKey = "RegionID", OtherKey = "RegionID")]
        public EntitySet<Territory> Territories
        {
            get 
            {
                return this.territories; 
            }
        
            set 
            { 
                this.territories.Assign(value); 
            }
        }
        
        protected virtual void SendPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        
        protected virtual void SendPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        private void AttachTerritories(Territory entity)
        {
            this.SendPropertyChanging("Territories");
            entity.Region = this;
        }
        
        private void DetachTerritories(Territory entity)
        {
            this.SendPropertyChanging("Territories");
            entity.Region = null;
        }
        
        #region Extensibility methods
        
        partial void OnCreated();
        
        partial void OnLoaded();
        
        partial void OnValidate(ChangeAction action);
        
        partial void OnRegionIDChanging(int value);
        
        partial void OnRegionIDChanged();
        
        partial void OnRegionDescriptionChanging(string value);
        
        partial void OnRegionDescriptionChanged();
        
        #endregion
    }
}