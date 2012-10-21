﻿using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Units
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(UnitType))]
    [KnownType(typeof(Symbol))]
    [KnownType(typeof(Modifier))]
    [Serializable()]
    public partial class Unit : INotifyPropertyChanged,IEquatable<Unit>
    {
        #region "Constructors"
		public Unit() : base()
		{
		}

		internal Unit(UnitType unitType)
		{
			if (unitType == null) {
				throw new ArgumentNullException("unitType");
			}

			this.UnitType = unitType;
		}

		public bool IsDefault {
			get {
				foreach ( Modifier in this.Modifiers) {
					if (Modifier.UnitSourceID == Modifier.UnitTargetID & Modifier.UnitSourceID == this.ID) {
						return true;
					}
				}

				return false;

				//If Me.Modifiers.Count > 1 Then
				//    Return False
				//Else
				//    If Me.Modifiers(0).Value = 1 AndAlso Me.Modifiers(0).ModifierType = ModifierType.Multiply 'Then
				//        Return True
				//    Else
				//        Return False
				//    End If
				//End If
			}
		}
		#endregion

        #region "Primitive Properties"

        [DataMember()]
        public int ID { get; set; }

        [DataMember()]
        public string Name { get; set; }

        [DataMember()]
        public int UnitTypeID { get; set; }

        #endregion

        #region "Navigation Properties"

        [DataMember()]
        public UnitType UnitType { get; set; }

        [DataMember()]
        public List<Symbol> Symbols
        {
            get
            {
                if (_symbols == null && IsSerializing == false)
                {
                    _symbols = new List<Symbol>();
                }
                return _symbols;
            }
            set
            {
                if (!object.ReferenceEquals(_symbols, value))
                {
                    if (_symbols != null)
                    {
                        _symbols.CollectionChanged -= FixupSymbols;
                    }
                    _symbols = value;
                    if (_symbols != null)
                    {
                        _symbols.CollectionChanged += FixupSymbols;
                    }
                    OnNavigationPropertyChanged("Symbols");
                }
            }
        }

        [DataMember()]
        public List<Modifier> Sources
        {
            get
            {
                if (_sources == null && IsSerializing == false)
                {
                    _sources = new List<Modifier>();
                    _sources.CollectionChanged += FixupSources;
                }
                return _sources;
            }
            set
            {
                if (!object.ReferenceEquals(_sources, value))
                {
                    if (_sources != null)
                    {
                        _sources.CollectionChanged -= FixupSources;
                    }
                    _sources = value;
                    if (_sources != null)
                    {
                        _sources.CollectionChanged += FixupSources;
                    }
                    OnNavigationPropertyChanged("Sources");
                }
            }
        }

        private List<Modifier> _sources;

        [DataMember()]
        public List<Modifier> Modifiers
        {
            get
            {
                if (_modifiers == null && IsSerializing == false)
                {
                    _modifiers = new List<Modifier>();
                }
                return _modifiers;
            }
            set
            {
                if (!object.ReferenceEquals(_modifiers, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_modifiers != null)
                    {
                        _modifiers.CollectionChanged -= FixupModifiers;
                    }
                    _modifiers = value;
                    if (_modifiers != null)
                    {
                        _modifiers.CollectionChanged += FixupModifiers;
                    }
                    OnNavigationPropertyChanged("Modifiers");
                }
            }
        }

        #endregion

        #region "ChangeTracking"

        private bool _isDeserializing;
        protected bool IsDeserializing
        {
            get { return _isDeserializing; }
            private set { _isDeserializing = value; }
        }

        [OnDeserializing()]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }

        [OnDeserialized()]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
        }

        private bool _isSerializing = false;
        protected bool IsSerializing
        {
            get { return _isSerializing; }
            private set { _isSerializing = value; }
        }

        [OnSerializing()]
        public void OnSerializingMethod(StreamingContext context)
        {
            IsSerializing = true;
        }

        [OnSerialized()]
        public void OnSerializedMethod(StreamingContext context)
        {
            IsSerializing = false;
        }

        protected virtual void ClearNavigationProperties()
        {
            UnitType = null;
            Symbols.Clear();
            Sources.Clear();
            Modifiers.Clear();
        }

        #endregion

        #region "Properties"
		public string DefaultSymbol {
			get {
				Symbol symbol = null;
				//symbol = (From s In Me.Symbols
				//          Where s.IsDefault = True).FirstOrDefault

				if (symbol == null) {
					return string.Empty;
				} else {
					return symbol.Value;
				}
			}
		}
		#endregion

        #region "IEquatable"
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (!object.ReferenceEquals(obj.GetType(), this.GetType()))
            {
                return false;
            }
            else
            {
                return this.Equals((Unit)obj);
            }
        }

        public bool Equals(Unit other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                if (this.Name != other.Name)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        public static bool operator !=(Unit left, Unit right)
        {
            if (left == null && right == null)
            {
                return true;
            }
            else if (left == null || right == null)
            {
                return true;
            }
            else
            {
                return !left.Equals(right);
            }
        }

        public static bool operator ==(Unit left, Unit right)
        {
            if (left == null && right == null)
            {
                return false;
            }
            else if (left == null || right == null)
            {
                return false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        #endregion
    }

}



	
		
	
		

		
		