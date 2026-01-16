using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Common
{
	public static class MsgEnc
	{
		public static byte[] enc(Object obj)
		{
			//byte[] bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
			string txt = JsonSerializer.Serialize(obj);

			byte[] bytes = Encoding.UTF8.GetBytes(txt);
			return bytes;
		}

		public static T? dec<T>(byte[] bytes)
		{
			string msg = Encoding.UTF8.GetString(bytes);
			try
			{
				T? obj = JsonSerializer.Deserialize<T>(msg);
				return obj;
			}
			catch
			{
				return default(T); //null
			}
		}
	}


	public enum ResponseCode
	{
		ok = 0,
		error = 1,
		busy = 2
	}

	public class kvMessage
	{
		public AccountInfo? accountInfo { get; set; } //only set by backend
		public string? clientId { get; set; } = string.Empty;
		public string? cmd { get; set; } = string.Empty;
		public Object? data { get; set; } = null;
		public Object? exData { get; set; } = null;

		public override string ToString()
		{
			return $"cmd={cmd}, clientid={clientId}, data={data}";
		}
	}

	public class MsgConnectionInfoData
	{
		public string? clientId { get; set; } = string.Empty;
		public string? accountId { get; set; } = string.Empty;
		public string? contactId { get; set; } = string.Empty;
		public string? accountType { get; set; } = string.Empty;
		public string? jwtToken { get; set; } = string.Empty;
		public string? hash { get; set; } = string.Empty;
	}


	public class MsgClientIdData
	{
		public string clientId { get; set; } = string.Empty;
	}

	public class ResponseData
	{
		public ResponseCode responseCode { get; set; } = ResponseCode.ok;
		public string responseText { get; set; } = string.Empty;
	}

	public class MsgLoginData
	{
		public string login { get; set; } = string.Empty;
		public string password { get; set; } = string.Empty;
	}

	public class MsgLoginResponseData
	{
		public bool loginGranted { get; set; }
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string accountType { get; set; } = string.Empty;
	}

	public class MsgLogoutData
	{
		//nothing?
	}

	public class MsgGetDeviceIdsData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
	}

    public class MsgGetDeviceIdsDataResponse
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;


		public List<string> deviceIds { get; set; } = new List<string>();
	}

    public class MsgGetDevicesData
    {
        public string accountId { get; set; } = string.Empty;
        public string contactId { get; set; } = string.Empty;
    }

    public class MsgGetDevicesDataResponse
    {
        public string accountId { get; set; } = string.Empty;
        public string contactId { get; set; } = string.Empty;
		public List<Device> devices { get; set; } = new List<Device>();
    }


    public class MsgRegisterAsOwnerData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;


		public string deviceId { get; set; } = string.Empty;
		public string deviceToken { get; set; } = string.Empty;
	}

	public class MsgDeleteOwnerData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
	}

	public class MsgGetOwnerData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
	}

	public class MsgGetOwnerDataResponse
	{
		public string deviceId { get; set; } = string.Empty;
		public string ownerAccountId { get; set; } = string.Empty;
		public string ownerContactId { get; set; } = string.Empty;
	}


	public class MsgAddObserverData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;


		public string deviceId { get; set; } = string.Empty;
		public DateTime startTime { get; set; }
		public DateTime endTime { get; set; }
		public string deviceToken { get; set; } = string.Empty;
	}

	public class MsgDeleteObserverData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
	}

	public class MsgObservedDevicesData
	{
		public string accountId { get; set; } = string.Empty; //tech!
		public string contactId { get; set; } = string.Empty; //tech!
		public string contactAccountId { get; set; } = string.Empty; //dentist!
		public string contactContactId { get; set; } = string.Empty; //dentist!
	}

	public class MsgGetObservedContactsData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
	}

	public class MsgGetObservedContactsDataResponse
	{
		public List<AccountContactInfo> contacts { get; set; } = new List<AccountContactInfo>();
		public string favoriteContactId { get; set; } = string.Empty;
	}

	public class MsgObservedDevicesDataResponse
	{
		public string accountId { get; set; } = string.Empty; //account if tech
		public string contactId { get; set; } = string.Empty; //contact if tech
		public string contactAccountId { get; set; } = string.Empty; //account if dentist
		public string contactContactId { get; set; } = string.Empty; //contact if dentist
		public List<string> deviceIds { get; set; } = new List<string>();
	}

	public class MsgObserversForDeviceData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;

		public string deviceId { get; set; } = string.Empty;
	}

	public class MsgGetAccountUserListData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
	}

	public class MsgGetAccountUserListDataResponse
	{
		public string accountId { get; set; } = string.Empty;
		public List<PortalUser> users { get; set; } = new List<PortalUser>();
	}



	public class MsgOwnOrObservedDeviceIds
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
	}

	public class MsgOwnOrObservedDeviceIdsReply
	{
		public string accountId { get; set; } = string.Empty;
		public List<string> deviceIds { get; set; } = new List<string>();
	}

	public class MsgAddTagsData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;

		public string deviceId { get; set; } = string.Empty;
		public List<Property> tags { get; set; } = new List<Property>();
	}
	public class MsgDeleteTagData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;

		public string deviceId { get; set; } = string.Empty;
		public List<Property> tags { get; set; } = new List<Property>();
	}

	public class MsgRenameDeviceTag
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string oldTagName { get; set; } = string.Empty;
		public string newTagName { get; set; } = string.Empty;
	}


	public class MsgMoveTagData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;

		public string deviceId { get; set; } = string.Empty;
		public int oldIndex { get; set; }
		public int newIndex { get; set; }
	}

	public class MsgGetTagsData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;

		public string deviceId { get; set; } = string.Empty;
	}

	public class MsgGetTagsResponseData
	{
		public string deviceId { get; set; } = string.Empty;
		public string accountId { get; set; } = string.Empty;
		public List<Property>? tags { get; set; } = new List<Property>();
	}




	public class MsgSendCommandData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;

		public string deviceId { get; set; } = string.Empty;
		public string command { get; set; } = string.Empty;
	}

	public class MsgGetDeviceInfoData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;

		public string deviceId { get; set; } = string.Empty;
	}

	public class MsgGetDeviceInfoDataResponse
	{
		public string deviceId { get; set; } = string.Empty;
		public DeviceInfoEntry? deviceInfo { get; set; } = new DeviceInfoEntry();
	}



	public class MsgGetDeviceStatusData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
	}

	public class MsgGetDeviceStatusResponseData
	{
		public string deviceId { get; set; } = string.Empty;
		public DeviceStatus status { get; set; } = new DeviceStatus();
	}


	public class MsgAccountInfoData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string accountType { get; set; } = string.Empty;
		public bool mustUpdate { get; set; } // magic bool to force db to check&update accountIds in tables
		public string jwtToken { get; set; } = string.Empty;
		public string hash { get; set; } = string.Empty;
	}




	public class MsgLiveData //start or stop the live stream (from client to core to device) and inform kvLive to consume images
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceToken { get; set; } = string.Empty; // only needed if authAccount is not owner or observer


		public string clientId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
		public string screenNames { get; set; } = string.Empty; //example "1" or "2" or "1,2" (with liveserver BINARY communication, only ONE screen per websocket is allowed)
		public bool active { get; set; }
		public string delay { get; set; } = string.Empty;
		public bool useLiveServer { get; set; } //always true!
	}

	public class MsgImageData // data frame from device to kvLive server
	{
		public string deviceId { get; set; } = string.Empty;
		public string screenName { get; set; } = string.Empty;
		public string imageData { get; set; } = string.Empty;
	}

	public class MsgImageDataBinary // data frame from device to kvLive server
	{
		public string deviceId { get; set; } = string.Empty;
		public int screenId { get; set; }
	}


	public class EventDeviceStatusChangedData
	{
		public string deviceId { get; set; } = string.Empty;
		public string valueName { get; set; } = string.Empty;
		public string valueValue { get; set; } = string.Empty;
	}

	public class EventDeviceStatusChangedByteData
	{
		public string deviceId { get; set; } = string.Empty;
		public string valueName { get; set; } = string.Empty;
		public byte[] valueValue { get; set; } = new byte[0];
	}


	public class MqttVerifyTokenMessage
	{
		public string cmd { get; set; } = string.Empty;
		public string token { get; set; } = string.Empty;
		public string msgCmd { get; set; } = string.Empty;
		public string? msgClientId { get; set; } = string.Empty;
		public Object? msgData { get; set; }
	}

	public class MsgSetExtendedDeviceInfo
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;

		public string deviceId { get; set; } = string.Empty;
		/*
		public string TuName { get; set; }
		public string TuImage { get; set;}
		*/
		public List<Property> extendedInfos { get; set; } = new List<Property>();
	}



	public class MsgGetHygieneReportData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
	}

	public class HygieneReportResponse
	{
		public List<HygieneReportData> reports { get; set; } = new List<HygieneReportData>();
	}

	public class MsgTuGetHygieneHashes
	{
		public string deviceId { get; set; } = string.Empty;
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
	}



	public class TuGetHygieneHashesResponseData
	{
		public int hashcount { get; set; }
		public List<HygieneHash> hygieneHashes { get; set; } = new List<HygieneHash>();
	}

	public class TuStringResponse
	{
		public string response { get; set; } = string.Empty;
	}

	public class MsgSyncHygieneReports
	{
		public string deviceId { get; set; } = string.Empty;
		public List<HygieneReportData> reports { get; set; } = new List<HygieneReportData>();
	}

	public class SyncHygieneReportsResponseData
	{
		public int syncResultCount { get; set; }
		public List<HygieneSyncResult> syncResults { get; set; } = new List<HygieneSyncResult>();
	}




	public class MsgStoreHygienePlanData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public HygienePlan hygienePlan { get; set; } = new HygienePlan();
	}

	public class MsgDeleteHygienePlanData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string hygienePlanId { get; set; } = string.Empty;
	}

	public class MsgGetHygienePlanData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string hygienePlanId { get; set; } = string.Empty;
	}

	public class MsgGetHygienePlanIdsData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
	}

	public class MsgGetHygienePlanIdsDataResponse
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public List<string> hyginePlanIds { get; set; } = new List<string>();
	}

    public class MsgGetHygienePlansData
    {
        public string accountId { get; set; } = string.Empty;
        public string contactId { get; set; } = string.Empty;
    }

    public class MsgGetHygienePlanDataResponse
    {
        public string accountId { get; set; } = string.Empty;
        public string contactId { get; set; } = string.Empty;
        public List<GetHygienePlanAccData> hygienePlans { get; set; } = new List<GetHygienePlanAccData>();
    }

    public class MsgAssignHygienePlan
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
		public string hygienePlanId { get; set; } = string.Empty;
	}

	public class MsgGetAssignedHygienePlanData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
	}

	public class GetAssignedHygienePlanResponseData
	{
		public string deviceId { get; set; } = string.Empty;
		public HygienePlan? hygienePlan { get; set; } = new HygienePlan();
	}

	public class GetHygienePlanAccData
	{
		public HygienePlan? hygienePlan { get; set; } = new HygienePlan();
		public List<string> assignedDevices { get; set; } = new List<string>();
	}

	public class SaveHygienePlanOnDeviceData
	{
		public HygienePlan? hygienePlan { get; set; } = null;
	}

	public class MsgContactData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string contactAccountId { get; set; } = string.Empty;
		public string contactContactId { get; set; } = string.Empty;
	}

	public class MsgGetDocumentsData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string accountType { get; set; } = string.Empty; //Praxis oder Depot etc.
		public string language { get; set; } = string.Empty;
		public string materialNumber { get; set; } = string.Empty;
	}

	public class MsgGetDocumentsDataResponse
	{
		public List<PdfDocument> documents { get; set; } = new List<PdfDocument>();
	}

	public class MsgGetContactbyIdResponse
	{
		public AccountContactInfo? accountContactInfo { get; set; } = new AccountContactInfo();
	}

	public class MsgGetTuConfigData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
	}

	public class MsgGetTuConfigDataResponse
	{
		public TuConfig config { get; set; } = new TuConfig();
	}

	public class MsgAddContactByMailData
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;


		public string contactEmail { get; set; } = string.Empty;
	}


	public class MsgUpsertContactNote
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;


		public string contactAccountId { get; set; } = string.Empty;
		public string contactContactId { get; set; } = string.Empty;


		public string NoteText { get; set; } = string.Empty;
		public string NoteId { get; set; } = string.Empty; //empty if new
	}

	public class MsgDeleteContactNote
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;


		public string contactAccountId { get; set; } = string.Empty;
		public string contactContactId { get; set; } = string.Empty;
		public string NoteId { get; set; } = string.Empty;
	}

	public class MsgGetDeviceNotes
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
	}

	public class MsgUpsertDeviceNote
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
		public string noteText { get; set; } = string.Empty;
		public string noteId { get; set; } = string.Empty;//empty if new
	}

	public class MsgDeleteDeviceNote
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
		public string noteId { get; set; } = string.Empty; //empty if new
	}

	public class MsgGetDeviceNotesResponse
	{
		public DeviceNotes notes { get; set; } = new DeviceNotes();
	}

	public class MsgGetNewJWT
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string hash { get; set; } = string.Empty;
	}

	public class MsgGetDentalOffices
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
	}

	public class MsgGetDentalOfficesResponse
	{
		public List<Office> offices { get; set; } = new List<Office>();
	}

	public class MsgSetContactAsFavorite
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string contactContactId { get; set; } = string.Empty;
	}

	public class MsgSetContactProperties
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string contactContactId { get; set; } = string.Empty;

		public bool isFavorite { get; set; }
		public bool hasAccess { get; set; }
	}

	public class MsgSetContactProperty
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
		public string contactContactId { get; set; } = string.Empty;
		public Property property { get; set; } = new Property();
		public bool deleteProperty { get; set; } //set to true to remove the property
	}

	public class MsgReady2Remote
	{
		public string deviceId { get; set; } = string.Empty;
		public bool ready2Remote { get; set; }
		public string hashPin { get; set; } = string.Empty;
		public string salt { get; set; } = string.Empty;
	}

	public class MsgReady2RemoteResponse
	{
		public ResponseCode responseCode { get; set; } = new ResponseCode();
		public string hashPin { get; set; } = string.Empty;
		public string entryId { get; set; } = string.Empty;
	}

	public class MsgOwnContactInOffice
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
	}

	public class MsgGetDeviceMessageList
	{
		public string deviceId { get; set; } = string.Empty;
	}


	public class MsgSetActiveOffice //for technician to set the observed office (events are passed to that client)
	{
		public string accountId { get; set; } = string.Empty;
		public string contactId { get; set; } = string.Empty;
	}

	public class MsgGetLoginDates
	{
		public string contactId { get; set; } = string.Empty;
	}


	public class MsgEnableDeviceActivation
	{
		public string? deviceId { get; set; } = string.Empty;
		public bool? whitelist { get; set; } = true;
		public string? reason { get; set; } = string.Empty;

	}

	public class MqttMsgGetDeviceMessageList
	{
		public string cmd { get; set; } = string.Empty;
		public string? clientId { get; set; } = string.Empty;
	}

	public class MsgSetDeviceMqttRootData
	{
		public string deviceId { get; set; } = string.Empty;
		public string mode { get; set; } = string.Empty;
		public string secret { get; set; } = string.Empty;
	}

	public class StartLiveResponseData
	{
		public ResponseCode responseCode { get; set; } = ResponseCode.ok;
		public string responseText { get; set; } = string.Empty;
		public string deviceId { get; set; } = string.Empty;
	}

    public class EventOfficeChangedData
    {
		public Office? office { get; set; }	
	}

	public class MsgSetTuImage
	{
		public string? deviceId { get; set; }
		public string? screenType { get; set; }
		public string? upholsteryType { get; set; }
		public string? upholsteryColor {  get; set; }
	}

	public class MsgDeleteContactAccountData
    {
		public string? contactId { get; set; }
	}
        
}
