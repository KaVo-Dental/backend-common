using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Data;

namespace Common
{


    public class AccountInfo
    {
        public const string ACCTYPEDENTALPROFESSIONAL = "Dental_Professional";
        public const string ACCTYPEDEALER = "Dealer";
        public const string ACCTYPEINSTITUTION = "Institution";
        public const string ACCTYPEDSO = "DSO";
        public const string ACCTYPECORPORATE = "Corporate";



        public string accountId
        {
            get { return myAccountId; }
            set { myAccountId = value; }
        }
        public string accountType
        {
            get { return myAccountType; }
            set { myAccountType = value; }
        }
        public bool isLogInGranted
        {
            get { return myIsLogInGranted; }
            set { myIsLogInGranted = value; }
        }

        public string contactId
        {
            get { return myContactId; }
            set { myContactId = value; }
        }


        public void setAccountInfo(string accountId, string accountType, string contactId, bool isLogInGranted)
        {
            this.accountId = accountId;
            this.accountType = accountType;
            this.contactId = contactId;
            this.isLogInGranted = isLogInGranted;
        }

        public void clear()
        {
            myAccountId = "";
            myAccountType = "";
            myContactId = "";
            myContactType = "";
            myIsLogInGranted = false;
        }

        public bool isValid()
        {
            if (string.IsNullOrEmpty(contactId) || !myIsLogInGranted)
                return false;

            return true;
        }

        private string myAccountId = ""; // the account if the login data
        private string myAccountType = "";
        private string myContactId = "";
        private string myContactType = ""; //role
        private bool myIsLogInGranted = false;
    }

    public class Property
    {
        public string Name { get; set; } = string.Empty; //example: "criticalerrors" or "inHygiene" or "inUse" etc....  
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        //example: Name="hasUpdate" Attributes=[version=1.2, date=1.1.2023] etc... 
        //         Name="UserRights" Attributes=[read=true, write=false] etc
        //         Name="Tag1" Attributes=[color=green , sticky=true]


        public static bool MergePropertyIntoList(List<Property> existingProperties, Property prop, bool deleteProperty)
        {
            if (existingProperties == null || prop == null || string.IsNullOrWhiteSpace(prop.Name))
                return false;

            var existingProp = existingProperties.FirstOrDefault(p => p.Name == prop.Name);

            if (deleteProperty)
            {
                if (existingProp != null)
                {
                    existingProperties.Remove(existingProp);
                    return true;
                }
                return false;
            }

            // add if property did not exist
            if (existingProp == null)
            {
                if (!string.IsNullOrWhiteSpace(prop.Name))
                {
                    existingProperties.Add(prop);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Merge attributes
            if (prop.Attributes != null)
            {
                if (existingProp.Attributes == null)
                    existingProp.Attributes = new Dictionary<string, string>();

                foreach (var kvp in prop.Attributes)
                {
                    existingProp.Attributes[kvp.Key] = kvp.Value; // overwrite or add
                }
            }

            return true;
        }

    }



    public class Error
    {
        public string ErrorId { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty; //example info, warning, error, critical ?
    }

    public class OwnerEntry
    {
        public string id { get; set; } = string.Empty; //==deviceId, cosmos index field 
        public string deviceId { get; set; } = string.Empty;
        public string accountId { get; set; } = string.Empty;  //this is the account that gained ownership
        public string contactId { get; set; } = string.Empty;
        public DateTime ownershipDate { get; set; }

        public string authAccountId { get; set; } = string.Empty; //this was the account that executed the ownership
        public string authContactId { get; set; } = string.Empty;
    }
    public class ObserverEntry
    {
        public string id { get; set; } = string.Empty; //every cosmos db entry has an id field
        public string deviceId { get; set; } = string.Empty; // <-- this is the partition key of that container table (so we can search, index, very fast on that property
        public string accountId { get; set; } = string.Empty;
        public string contactId { get; set; } = string.Empty;
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; } 
        public string authAccountId { get; set; } = string.Empty;
        public string authContactId { get; set; } = string.Empty;
    }

    public class DeviceTagEntry
    {
        public string id { get; set; } = string.Empty; //== deviceid
        public List<Property> tags { get; set; } = new List<Property>();
    }


    public class DeviceInfoEntry
    {
        public string id { get; set; } = string.Empty; //=deviceId, field is named as deviceId because of cosmosdb id field

        public string deviceProductName { get; set; } = string.Empty;
        public string deviceMaterialNumber { get; set; } = string.Empty;
        public string deviceSerialNumber { get; set; } = string.Empty;
        public string firmwareVersionSet { get; set; } = string.Empty;
        public List<Property>? extendedInfos { get; set; } = new List<Property>(); // additional Device Information can be stored here (example LocalIp etc.)
        public Dictionary<string,bool>? licenses { get; set; } = new Dictionary<string,bool>();
        public void copyFrom(DeviceInfoEntry entry, bool overwriteExtentedInfo = true)
        {
            this.id = entry.id;
            this.deviceProductName = entry.deviceProductName;
            this.deviceMaterialNumber = entry.deviceMaterialNumber;
            this.deviceSerialNumber = entry.deviceSerialNumber;
            this.firmwareVersionSet = entry.firmwareVersionSet;

            if (entry.extendedInfos != null && entry.extendedInfos.Count > 0)
            {
                if (this.extendedInfos == null)
                    this.extendedInfos = new List<Property>();

                foreach (Property property in entry.extendedInfos)
                {
                    Property.MergePropertyIntoList(this.extendedInfos, property, false);
                }
            }

            if (overwriteExtentedInfo && entry!=null && entry.extendedInfos!=null)
                this.extendedInfos = new List<Property>(entry.extendedInfos); //create a copy of the old list //todo: check: better merge into existing list?
        }
    }


    public class DeviceStatusHygieneState
    {
        public bool? isInHygiene { get; set; }
        public int? phase { get; set; }
        public int? program {  get; set; }
    }

    public class Notification
    {
        public string id { get; set; } = string.Empty;
        public string? category { get; set; } = string.Empty;
        public int? level { get; set; } = 0; //not set
        public string? title { get; set; } = string.Empty;
        public string? topText { get; set; } = string.Empty;
        public string? bottomText { get; set; } = string.Empty;

        //"old" properties (to be removed after sweden gets final version)
        public string? description { get; set; } = string.Empty;
        public string? errorNumber { get; set; } = string.Empty;
        public string? text { get; set; } = string.Empty;
    }

    public class DeviceMessage //this is the list of messages that is stored in the device
    {
        public int id { get; set; }
        public string messageText { get; set; } = string.Empty;
        public DateTime dateTime { get; set; }
        public int counter { get; set; }
    }


    public class Location
    {
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;
        public double? Altitude { get; set; } = 0.0;

        public bool IsValid()
        {
            // Prüfe, ob Latitude und Longitude im gültigen Bereich liegen
            bool isLatValid = Latitude >= -90 && Latitude <= 90;
            bool isLonValid = Longitude >= -180 && Longitude <= 180;

            // Optional: Altitude kann null sein, aber wenn vorhanden, sollte sie sinnvoll sein
            bool isAltValid = !Altitude.HasValue || (Altitude.Value >= -500 && Altitude.Value <= 10000);

            return isLatValid && isLonValid && isAltValid;
        }

        public bool IsDefault()
        {
            if (Latitude==0.0 && Longitude==0.0)
                return false;

            return true;
        }
    }

    public class Device
    {
        public string deviceId { get; set; }
        public DeviceStatus? status { get; set; } = null;
        public List<Property>? tags { get; set; } = null;
        public DeviceInfoEntry? info { get; set; } = null;
    }


    public class DeviceStatus
    {
        public string id { get; set; } = string.Empty;
        public string deviceId { get; set; } = string.Empty;
        

        public bool? isOnline { get; set; } = null;//if false, all other properties are not valid (=empty)
        public DateTime? onlineTimeStartUtc { get; set; } = null;//last time when device turned online
        public DateTime? offlineTimeStartUtc { get; set; } = null;//last time when device turned offline
        public string? localIp { get; set; } = null;// last local ip that the device reported


        public bool? isInHygiene { get; set; } = null;// true if a hygiene cycle is active
        public DeviceStatusHygieneState? hygieneState { get; set; } = null;// desciption of state 
        public int? hygieneRemainingRestTime { get; set; } = null;//seconds if intensive germ reduction rest phase is active
        public string? hygieneCurrentCycle { get; set; } = null;// current hygiene cycle if hygiene is active

        public DateTime? hygieneLastMorningCycle {  get; set; } = null;//last completeted 'Morning Cycle' (UTC)
        public DateTime? hygieneLastEveningCycle { get; set; } = null;//last completeted 'Evening Cycle' (UTC)
        public DateTime? hygieneLastWeeklyCycle { get; set; } = null;//last completeted 'Weekly Cycle' (UTC)
        public DateTime? hygieneLastAfterTreatmentCycle { get; set; } = null;//last completeted 'After Treatment Cycle' (UTC)
        public string? hygienePlanName { get; set; } = null;// name of the hygiene plan (stored on the device!)


        public List<Notification>? criticalErrors { get; set; } = null;
        public List<Notification>? notifications { get; set; } = null;
        public List<Notification>? otherNotifications { get; set; } = null;//same category as notifications?
        public List<Notification>? hygieneNotifications { get; set; } = null;


        public bool? isInUse { get; set; } = null;
        public string? usedBy { get; set; } = null;


        public int? dekaseptolStatus { get; set; } = null;
        public int? oxygenalStatus { get; set; } = null;
        public int? footControlBatteryStatus { get; set; } = null;
        public DateTime? serviceInterval { get; set; } = null;//local time(!) of the device
        public List<DeviceMessage>? deviceMessageList { get; set; } = null; // this is the message list from the device
        public string? remotePin { get; set; } = null;

        public List<Property>? extendedStatusFields { get; set; } = null;

        public Location? deviceLocation {  get; set; } = null;

        public bool? isWaterBottleSystem { get; set; } = null;

        public bool copyFrom(DeviceStatus entry, bool copyNullValues = false)
        {
            bool anyValueChanged = false;

            this.id = entry.id;
            this.deviceId = entry.deviceId;

            if (copyNullValues || entry.isWaterBottleSystem != null)
            {
                if (this.isWaterBottleSystem != entry.isWaterBottleSystem)
                    anyValueChanged = true;
                this.isWaterBottleSystem = entry.isWaterBottleSystem;
            }

            if (copyNullValues || entry.deviceLocation != null /*&& entry.deviceLocation.IsValid()*/)
            {
                if (this.deviceLocation != entry.deviceLocation)
                    anyValueChanged = true;
                this.deviceLocation = entry.deviceLocation;
            }

            if (copyNullValues || entry.isOnline != null)
            {
                if (this.isOnline != entry.isOnline)
                    anyValueChanged = true;
                this.isOnline = entry.isOnline;
            }
            if (copyNullValues || entry.onlineTimeStartUtc != null)
            {
                if (this.onlineTimeStartUtc != entry.onlineTimeStartUtc)
                    anyValueChanged = true;
                this.onlineTimeStartUtc = entry.onlineTimeStartUtc;
            }
            if (copyNullValues || entry.offlineTimeStartUtc != null)
            {
                if (this.offlineTimeStartUtc != entry.offlineTimeStartUtc)
                    anyValueChanged = true;
                this.offlineTimeStartUtc = entry.offlineTimeStartUtc;
            }
            if (copyNullValues || entry.localIp != null)
            {
                if (this.localIp != entry.localIp)
                    anyValueChanged = true;
                this.localIp = entry.localIp;
            }

            if (copyNullValues || entry.isInHygiene != null)
            {
                if (this.isInHygiene != entry.isInHygiene)
                    anyValueChanged = true;
                this.isInHygiene = entry.isInHygiene;
            }

            if (copyNullValues || entry.hygieneState != null)
            {
                if (this.hygieneState != entry.hygieneState)
                    anyValueChanged = true;
                this.hygieneState = entry.hygieneState;
            }

            if (copyNullValues || entry.hygieneRemainingRestTime != null)
            {
                if (this.hygieneRemainingRestTime != entry.hygieneRemainingRestTime)
                    anyValueChanged = true;
                this.hygieneRemainingRestTime = entry.hygieneRemainingRestTime;
            }

            if (copyNullValues || entry.hygieneCurrentCycle != null)
            {
                if (this.hygieneCurrentCycle != entry.hygieneCurrentCycle)
                    anyValueChanged = true;
                this.hygieneCurrentCycle = entry.hygieneCurrentCycle;
            }

            if (copyNullValues || entry.hygieneLastMorningCycle != null)
            {
                if (this.hygieneLastMorningCycle != entry.hygieneLastMorningCycle)
                    anyValueChanged = true;
                this.hygieneLastMorningCycle = entry.hygieneLastMorningCycle;
            }

            if (copyNullValues || entry.hygieneLastEveningCycle != null)
            {
                if (this.hygieneLastEveningCycle != entry.hygieneLastEveningCycle)
                    anyValueChanged = true;
                this.hygieneLastEveningCycle = entry.hygieneLastEveningCycle;
            }

            if (copyNullValues || entry.hygieneLastWeeklyCycle != null)
            {
                if (this.hygieneLastWeeklyCycle != entry.hygieneLastWeeklyCycle)
                    anyValueChanged = true;
                this.hygieneLastWeeklyCycle = entry.hygieneLastWeeklyCycle;
            }

            if (copyNullValues || entry.hygieneLastAfterTreatmentCycle != null)
            {
                if (this.hygieneLastAfterTreatmentCycle != entry.hygieneLastAfterTreatmentCycle)
                    anyValueChanged = true;
                this.hygieneLastAfterTreatmentCycle = entry.hygieneLastAfterTreatmentCycle;
            }

            if (copyNullValues || entry.hygienePlanName != null)
            {
                if (this.hygienePlanName != entry.hygienePlanName)
                    anyValueChanged = true;
                this.hygienePlanName = entry.hygienePlanName;
            }

            if (copyNullValues || entry.criticalErrors != null)
            {
                if (this.criticalErrors != entry.criticalErrors)
                    anyValueChanged = true;
                this.criticalErrors = entry.criticalErrors;
            }

            if (copyNullValues || entry.notifications != null)
            {
                if (this.notifications != entry.notifications)
                    anyValueChanged = true;
                this.notifications = entry.notifications;
            }

            if (copyNullValues || entry.hygieneNotifications != null)
            {
                if (this.hygieneNotifications != entry.hygieneNotifications)
                    anyValueChanged = true;
                this.hygieneNotifications = entry.hygieneNotifications;
            }

            if (copyNullValues || entry.isInUse != null)
            {
                if (this.isInUse != entry.isInUse)
                    anyValueChanged = true;
                this.isInUse = entry.isInUse;
            }

            if (copyNullValues || entry.usedBy != null)
            {
                if (this.usedBy != entry.usedBy)
                    anyValueChanged = true;
                this.usedBy = entry.usedBy;
            }

            if (copyNullValues || entry.dekaseptolStatus != null)
            {
                if (this.dekaseptolStatus != entry.dekaseptolStatus)
                    anyValueChanged = true;
                this.dekaseptolStatus = entry.dekaseptolStatus;
            }

            if (copyNullValues || entry.oxygenalStatus != null)
            {
                if (this.oxygenalStatus != entry.oxygenalStatus)
                    anyValueChanged = true;
                this.oxygenalStatus = entry.oxygenalStatus;
            }

            if (copyNullValues || entry.footControlBatteryStatus != null)
            {
                if (this.footControlBatteryStatus != entry.footControlBatteryStatus)
                    anyValueChanged = true;
                this.footControlBatteryStatus = entry.footControlBatteryStatus;
            }

            if (copyNullValues || entry.serviceInterval != null)
            {
                anyValueChanged = true;
                this.serviceInterval = entry.serviceInterval;
            }

            if (copyNullValues || entry.deviceMessageList != null)
            {
                if (this.deviceMessageList != entry.deviceMessageList)
                    anyValueChanged = true;
                this.deviceMessageList = entry.deviceMessageList;
            }

            if (copyNullValues || entry.remotePin != null)
            {
                if (this.remotePin != entry.remotePin)
                    anyValueChanged = true;
                this.remotePin = entry.remotePin;
            }
           /* 
            if (copyNullValues && entry.extendedStatusFields == null)
            {
                if (this.extendedStatusFields != null)
                    anyValueChanged = true;

                this.extendedStatusFields = null; //but why should we do this?
            }
            else*/
            if (copyNullValues || entry.extendedStatusFields != null)
            {
                if (copyNullValues && entry.extendedStatusFields == null)
                {
                    this.extendedStatusFields = null;
                }
                else
                {
                    if (this.extendedStatusFields == null)
                    {
                        anyValueChanged = true;
                        this.extendedStatusFields = new List<Property>();
                    }

                    if (entry.extendedStatusFields != null)
                    {
                        for (int i = 0; i < entry.extendedStatusFields.Count; i++)
                        {
                            Property p = entry.extendedStatusFields[i];
                            bool found = false;
                            foreach (Property p2 in this.extendedStatusFields)
                            {
                                if (p2.Name.Equals(p.Name))
                                {
                                    if (p2.Attributes != p.Attributes)
                                        anyValueChanged = true;

                                    p2.Attributes = p.Attributes;
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                anyValueChanged = true;
                                entry.extendedStatusFields.Add(new Property()
                                {
                                    Name = p.Name,
                                    Attributes = p.Attributes,
                                });
                            }
                        }
                    }
                }
            }

            return anyValueChanged;
        }
    }


    public class DeviceMessageEntry    
    {
        public string text { get; set; } = string.Empty;
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        public int counter { get; set; }
    }

    public class DeviceMessageList
    {
        public string id => deviceId;
        public string deviceId { get; set; } = string.Empty;
        public List<DeviceMessageEntry> entries { get; set; } = new List<DeviceMessageEntry>();
        public DateTime lastUpdateDate { get; set; }
    }


    public enum HygieneEvent
    {
        MorningStart,
        MorningEnd,
		EveningStart,
		EveningEnd,
		AfterTreatmentStart,
		AfterTreatmentEnd,
        WeeklyStart,
        WeeklyRestPhaseStart,
        WeeklyRestPhaseEnd,
        WeeklyEnd,
	}

    public class HygieneReportRawData
    {
        public DateTime? datetime { get; set; }
        public HygieneEvent hygieneEvent { get; set; }
    }

	public class HygieneReportData
	{
		public string id { get; set; } = string.Empty; //=deviceId_date (can be kept empty by device (server will overwrite/set this entry when storing data)
		public string deviceId { get; set; } = string.Empty;
		public DateTime date { get; set; }
        public string hash { get; set; } = string.Empty; //not a real hash, only the raw events (int value) as string
		public bool MorningDone { get; set; }
		public bool EveningDone { get; set; }

        public bool WeeklyRestphaseStarted { get; set; }
        public bool WeeklyDone { get; set; }
        public int AfterTreatmentCount { get; set; }
        public List<HygieneReportRawData> rawEventData { get; set; } = new List<HygieneReportRawData>();
	}

	public class HygieneHash
	{
		public DateTime date { get; set; }
		public string hash { get; set; } = string.Empty;
	}

	public class HygieneSyncResult
    {
        public DateTime hygieneDate { get; set; }
        public bool synced { get; set; }
        public string errorText { get; set; } = string.Empty;
    }

    public class Semesterbreak 
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class TimePlan
    {
        public string MorningHygiene { get; set; } = string.Empty;
        public string EveningHygiene { get; set; } = string.Empty;
        public string WeeklyHygiene { get; set; } = string.Empty;
    }

    public enum HygieneRuleType
    {
        Boolean,
        Integer,
        Text,
    }

    public class HygieneRule
    {
        public string HygieneRuleName { get; set; } = string.Empty;
        public HygieneRuleType HygieneRuleType { get; set; }
		public bool? ValueBool { get; set; }
		public int? ValueInt { get; set; }
		public String? ValueText { get; set; } = String.Empty;
	}

    public class HygienePlan
    {
        public string Id { get; set; } = string.Empty;
        //general
        public string HygienePlanName { get; set; } = string.Empty;

		//rules
		public List<HygieneRule> HygieneRules { get; set; } = new List<HygieneRule>();
		
		//semester break
		public List<Semesterbreak> SemesterBreaks { get; set; } = new List<Semesterbreak>();

        //timeplan
		public List<TimePlan> PlannedHygieneCycles { get; set; } = new List<TimePlan>(); //list with 7 elements for a week

		public DateTime saveDateTime { get; set; }

		public bool isEqual(HygienePlan other) 
        {
            if (other==null)
                return false;

            if (!HygienePlanName.Equals(other.HygienePlanName) ||
                (HygieneRules.Count != other.HygieneRules.Count) ||
                (SemesterBreaks.Count != other.SemesterBreaks.Count) ||
                (PlannedHygieneCycles.Count != other.PlannedHygieneCycles.Count))
                return false;

            for(int i = 0;i< HygieneRules.Count;i++) 
            {
                if (!HygieneRules[i].HygieneRuleName.Equals(other.HygieneRules[i].HygieneRuleName) ||
					HygieneRules[i].HygieneRuleType != other.HygieneRules[i].HygieneRuleType ||
					HygieneRules[i].ValueBool != other.HygieneRules[i].ValueBool ||
					HygieneRules[i].ValueText != other.HygieneRules[i].ValueText ||
					HygieneRules[i].ValueInt != other.HygieneRules[i].ValueInt
					)
                    return false;
            }

            for (int i = 0; i < SemesterBreaks.Count; i++)
            {
                if (SemesterBreaks[i].Start != other.SemesterBreaks[i].Start ||
					SemesterBreaks[i].End != other.SemesterBreaks[i].End) 
                    return false;
            }

            for (int i = 0; i < PlannedHygieneCycles.Count; i++)
            {
                if (!PlannedHygieneCycles[i].MorningHygiene.Equals(other.PlannedHygieneCycles[i].MorningHygiene) ||
                    !PlannedHygieneCycles[i].EveningHygiene.Equals(other.PlannedHygieneCycles[i].EveningHygiene) ||
                    !PlannedHygieneCycles[i].WeeklyHygiene.Equals(other.PlannedHygieneCycles[i].WeeklyHygiene))
                    return false;
			}
				return true;
        }


        public static HygienePlan sanitizePlan(HygienePlan plan)
        {
            foreach (TimePlan tp in plan.PlannedHygieneCycles)
            {
                tp.MorningHygiene = NormalizeTime(tp.MorningHygiene);
                tp.EveningHygiene = NormalizeTime(tp.EveningHygiene);
                tp.WeeklyHygiene = NormalizeTime(tp.WeeklyHygiene);

                //only evening or weekly!
                if (!string.IsNullOrWhiteSpace(tp.WeeklyHygiene) && !string.IsNullOrWhiteSpace(tp.EveningHygiene))
                {
                    tp.EveningHygiene = string.Empty;
                }
            }
            return plan;
        }
    

        private static string NormalizeTime(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            // Prüfen, ob überhaupt Ziffern enthalten sind
            if (!input.Any(char.IsDigit))
                return "";

            var parts = input.Split(':');
            int hour = 0, minute = 0;

            if (parts.Length >= 1 && int.TryParse(parts[0], out int h))
                hour = h;
            if (parts.Length >= 2 && int.TryParse(parts[1], out int m))
                minute = m;

            // Normalize values
            if (hour < 0) hour = 0;
            if (minute < 0) minute = 0;

            if (hour > 23) hour = 0;       // Alles > 23 wird 0
            if (minute > 59) minute = 59;  // Minuten > 59 werden 59

            return $"{hour:D2}:{minute:D2}";
        }
    }

    public class DbHygienePlan
    {
        public string id { get; set; } = string.Empty;
        public string accountId { get; set; } = string.Empty;
        public string contactId { get; set; } = string.Empty;
        public HygienePlan hygienePlan { get; set; } = new HygienePlan();
    }

	public class DbDeviceHygienePlan
	{
        public string id { get; set; } = string.Empty; //=deviceId
		public string hygienePlanId { get; set; } = string.Empty;
	}

    public class PortalUser
    {
        [JsonPropertyName("id")]
        public string id => contactId;

        public string accountId { get; set; } = string.Empty;
        public string contactId { get; set; } = string.Empty;


        // cdc account info of that user
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string company { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string zip { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string phone_business { get; set; } = string.Empty;
        public string phone_mobile { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public bool? verifiedMail { get; set; }


        public DateTime LastLogIn { get; set; }
    }


    public class Office
    {
        public string id { get; set; } = string.Empty;
        public string accountId { get; set; } = string.Empty;
        public string contactId { get; set; } = string.Empty;

        public PortalUser officeData { get; set; } = new PortalUser();

        // global status of all devices of that office
        public List<string> infosCritical { get; set; } = new List<string>();//list contains deviceids (in order to remove the entries if a devie is turned OFF
        public List<string> infosHygiene { get; set; } = new List<string>();
        public List<string> infosWarning { get; set; } = new List<string>();


        //some status stuff
        public DateTime lastLogin { get; set; } = DateTime.MinValue;
        public string lastLoggedInContactId { get; set; } = string.Empty;
        public bool isLoggedIn { get; set; } = false;


        // contacts of the office/user
        public string favoriteContact { get; set; } = string.Empty;
        public List<AccountContactInfo> contacts { get; set; } = new List<AccountContactInfo>();


        public void removeUnneccessaryData() //technician does not get more than necessary data from dentist office (only contact + status)
        {
            if (officeData!=null)
                officeData.LastLogIn = DateTime.MinValue; 
            lastLoggedInContactId = string.Empty;
            favoriteContact = string.Empty;
            lastLogin = DateTime.MinValue;
            contacts = new List<AccountContactInfo>();
            isLoggedIn = false;
        }
    }

    public class AccountContactInfo
    {
        public string accountId { get; set; } = string.Empty;
        public string contactId { get; set; } = string.Empty;
        //public Dictionary<string, string> contactInfo { get; set; }
        public PortalUser contactdata { get; set; } = new PortalUser(); //empty in db, will be filled from portaluser table data
        public List<Note> notes { get; set; } = new List<Note>();
        public List<Property> properties { get; set; } = new List<Property>();
    }

    public class Note
    {
        public string id { get; set; } = string.Empty;
        public DateTime createdDateTime { get; set; }
        public DateTime modifiedDateTime { get; set; }
        public string creatorAccountId { get; set; } = string.Empty;
        public string creatorContactId { get; set; } = string.Empty;
        public string lasteditorAccountId { get; set; } = string.Empty;
        public string lasteditorContactId { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
        public string? author { get; set; }
    }


    public class PdfDocument
    {
        public string FileName { get; set; } = string.Empty;
        public List<string> SKUPartNumber { get; set; } = new List<string>();
        public string DocumentType { get; set; } = string.Empty;
        public List<string> Language { get; set; } = new List<string>();
        public List<string> Country { get; set; } = new List<string>();
        public string Title { get; set; } = string.Empty;
        public string DocumentViewerUrl { get; set; } = string.Empty;
        public string OriginalUrl { get; set; } = string.Empty;
        public int FileSize { get; set; } = 0; //kByte
        public int PageCount { get; set; } = 0;
    }


    
    public class TuConfigEntry
    {
        public string ATINN { get; set; } = string.Empty;
        public string ATNAM { get; set; } = string.Empty;
        public string ATBEZ { get; set; } = string.Empty;
        public string ATWRT { get; set; } = string.Empty;
        public string ATWTB { get; set; } = string.Empty;
        public double EWAHR { get; set; }
    }

    public class License
    {
        public string Name { get; set; } = string.Empty;
        public bool? isConfigured { get; set; }
        public bool? isInstalled { get; set; }
    }

    public class DeviceLicenses
    {
        public string id { get; set; } = string.Empty;
        public List<License> licenses { get; set; } = new List<License>();
    }

    public class TuConfig
    {
        public string id { get; set; } = string.Empty;
        public List<TuConfigEntry>? items { get; set; } = new List<TuConfigEntry>();
        public List<License>? licenses { get; set; } = new List<License>();
    }

    public class TuConfiguration
    {
        public string id { get; set; } = string.Empty;
        public List<TuConfigEntry> items { get; set; } = new List<TuConfigEntry>();
    }

    public class DeviceNotes
    {
        public string id { get; set; } = string.Empty; //deviceId
        public List<Note> notes { get; set; } = new List<Note>();
    }

    public class RemoteDevice
    {
        [JsonPropertyName("id")]
        public string id => entryId;
        public string entryId { get; set; } = string.Empty;
        public string hashPin { get; set; } = string.Empty;
        public string salt { get; set; } = string.Empty;
        public string deviceId { get; set; } = string.Empty;
        public DateTime dateTime { get; set; }
        public string urlName { get; set; } = string.Empty;
    }

    public class LoginDates
    {
        [JsonPropertyName("id")]
        public string id => contactId;
        public string contactId { get; set; } = string.Empty;
        public List<DateTime> logindates { get; set; } = new List<DateTime>();
    }

    public class Base64Data
    {
        public string data { get; set; } = string.Empty;
    }
}
