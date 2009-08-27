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

    [Table(Name = "dbo.Shippers")]
    public partial class Shipper : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int shipperID;
        private string companyName;
        private string phone;
        private EntitySet<Order> orders;
        
        public Shipper()
        {
            this.orders = new EntitySet<Order>(this.AttachOrders, this.DetachOrders);
            this.OnCreated();
        }
        
        public event PropertyChangingEventHandler PropertyChanging;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [Column(Name = "ShipperID", Storage = "shipperID", CanBeNull = false, DbType = "Int NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int ShipperID
        {
            get
            {
                return this.shipperID;
            }
        
            set
            {
                if (this.shipperID != value)
                {
                    this.OnShipperIDChanging(value);
                    this.SendPropertyChanging("ShipperID");
                    this.shipperID = value;
                    this.SendPropertyChanged("ShipperID");
                    this.OnShipperIDChanged();
                }
            }
        }

        [Column(Name = "CompanyName", Storage = "companyName", CanBeNull = false, DbType = "NVarChar(40) NOT NULL")]
        public string CompanyName
        {
            get
            {
                return this.companyName;
            }
        
            set
            {
                if (this.companyName != value)
                {
                    this.OnCompanyNameChanging(value);
                    this.SendPropertyChanging("CompanyName");
                    this.companyName = value;
                    this.SendPropertyChanged("CompanyName");
                    this.OnCompanyNameChanged();
                }
            }
        }

        [Column(Name = "Phone", Storage = "phone", CanBeNull = true, DbType = "NVarChar(24)")]
        public string Phone
        {
            get
            {
                return this.phone;
            }
        
            set
            {
                if (this.phone != value)
                {
                    this.OnPhoneChanging(value);
                    this.SendPropertyChanging("Phone");
                    this.phone = value;
                    this.SendPropertyChanged("Phone");
                    this.OnPhoneChanged();
                }
            }
        }

        [Association(Name = "Shipper_Order", Storage = "orders", ThisKey = "ShipperID", OtherKey = "ShipVia")]
        public EntitySet<Order> Orders
        {
            get 
            {
                return this.orders; 
            }
        
            set 
            { 
                this.orders.Assign(value); 
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
        
        private void AttachOrders(Order entity)
        {
            this.SendPropertyChanging("Orders");
            entity.Shipper = this;
        }
        
        private void DetachOrders(Order entity)
        {
            this.SendPropertyChanging("Orders");
            entity.Shipper = null;
        }
        
        #region Extensibility methods
        
        partial void OnCreated();
        
        partial void OnLoaded();
        
        partial void OnValidate(ChangeAction action);
        
        partial void OnShipperIDChanging(int value);
        
        partial void OnShipperIDChanged();
        
        partial void OnCompanyNameChanging(string value);
        
        partial void OnCompanyNameChanged();
        
        partial void OnPhoneChanging(string value);
        
        partial void OnPhoneChanged();
        
        #endregion
    }
}
