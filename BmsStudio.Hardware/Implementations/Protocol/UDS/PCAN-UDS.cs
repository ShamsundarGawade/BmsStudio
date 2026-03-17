//  PCAN-UDS.cs
//
//  ~~~~~~~~~~~~
//
//  PCAN-UDS API
//
//  ~~~~~~~~~~~~
//
//  ------------------------------------------------------------------
//  Author : Fabrice Vergnaud
//	Last changed by:	$Author: Fabrice $
//  Last changed date:	$Date: 2019-10-11 14:05:33 +0200 (Fri, 11 Oct 2019) $
//
//  Language: C#
//  ------------------------------------------------------------------
//
//  Copyright (C) 2015  PEAK-System Technik GmbH, Darmstadt
//  more Info at http://www.peak-system.com 
//
using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Peak.Can.Uds
{
    // Aliases definition
    //
    using TPUDSCANHandle = System.UInt16;     // Represents a CAN hardware channel of the underlying CAN system

    #region Enumerations
    /// <summary>
    /// Represents a PCAN Baud rate register value
    /// </summary>
    public enum TPUDSBaudrate : ushort
    {
        /// <summary>
        /// 1 MBit/s
        /// </summary>
        PUDS_BAUD_1M = 0x0014,
        /// <summary>
        /// 800 kBit/s
        /// </summary>
        PUDS_BAUD_800K = 0x0016,
        /// <summary>
        /// 500 kBit/s
        /// </summary>
        PUDS_BAUD_500K = 0x001C,
        /// <summary>
        /// 250 kBit/s
        /// </summary>
        PUDS_BAUD_250K = 0x011C,
        /// <summary>
        /// 125 kBit/s
        /// </summary>
        PUDS_BAUD_125K = 0x031C,
        /// <summary>
        /// 100 kBit/s
        /// </summary>
        PUDS_BAUD_100K = 0x432F,
        /// <summary>
        /// 95,238 kBit/s
        /// </summary>
        PUDS_BAUD_95K = 0xC34E,
        /// <summary>
        /// 83,333 kBit/s
        /// </summary>
        PUDS_BAUD_83K = 0x852B,
        /// <summary>
        /// 50 kBit/s
        /// </summary>
        PUDS_BAUD_50K = 0x472F,
        /// <summary>
        /// 47,619 kBit/s
        /// </summary>
        PUDS_BAUD_47K = 0x1414,
        /// <summary>
        /// 33,333 kBit/s
        /// </summary>
        PUDS_BAUD_33K = 0x8B2F,
        /// <summary>
        /// 20 kBit/s
        /// </summary>
        PUDS_BAUD_20K = 0x532F,
        /// <summary>
        /// 10 kBit/s
        /// </summary>
        PUDS_BAUD_10K = 0x672F,
        /// <summary>
        /// 5 kBit/s
        /// </summary>
        PUDS_BAUD_5K = 0x7F7F,
    }

    /// <summary>
    /// Represents the different Not Plug-And-Play PCAN Hardware types
    /// </summary>
    public enum TPUDSHWType : byte
    {
        /// <summary>
        /// PCAN-ISA 82C200
        /// </summary>
        PUDS_TYPE_ISA = 0x01,
        /// <summary>
        /// PCAN-ISA SJA1000
        /// </summary>
        PUDS_TYPE_ISA_SJA = 0x09,
        /// <summary>
        /// PHYTEC ISA 
        /// </summary>
        PUDS_TYPE_ISA_PHYTEC = 0x04,
        /// <summary>
        /// PCAN-Dongle 82C200
        /// </summary>
        PUDS_TYPE_DNG = 0x02,
        /// <summary>
        /// PCAN-Dongle EPP 82C200
        /// </summary>
        PUDS_TYPE_DNG_EPP = 0x03,
        /// <summary>
        /// PCAN-Dongle SJA1000
        /// </summary>
        PUDS_TYPE_DNG_SJA = 0x05,
        /// <summary>
        /// PCAN-Dongle EPP SJA1000
        /// </summary>
        PUDS_TYPE_DNG_SJA_EPP = 0x06,
    }

    /// <summary>
    /// Represent the PUDS error and status codes 
    /// </summary>
    public enum TPUDSStatus : uint
    {
        /// <summary>
        /// No error 
        /// </summary>
        PUDS_ERROR_OK = 0x00000,
        /// <summary>
        /// Not Initialized
        /// </summary>
        PUDS_ERROR_NOT_INITIALIZED = 0x00001,
        /// <summary>
        /// Already Initialized
        /// </summary>
        PUDS_ERROR_ALREADY_INITIALIZED = 0x00002,
        /// <summary>
        /// Could not obtain memory
        /// </summary>
        PUDS_ERROR_NO_MEMORY = 0x00003,
        /// <summary>
        /// Input buffer overflow
        /// </summary>
        PUDS_ERROR_OVERFLOW = 0x00004,
        /// <summary>
        /// Timeout while accessing the PCANTP mutex
        /// </summary>
        PUDS_ERROR_TIMEOUT = 0x00006,
        /// <summary>
        /// No Message available
        /// </summary>
        PUDS_ERROR_NO_MESSAGE = 0x00007,
        /// <summary>
        /// Wrong message parameters
        /// </summary>
        PUDS_ERROR_WRONG_PARAM = 0x00008,
        /// <summary>
        /// PCANTP Channel is in BUS-LIGHT error state
        /// </summary>
        PUDS_ERROR_BUSLIGHT = 0x00009,
        /// <summary>
        /// PCANTP Channel is in BUS-HEAVY error state
        /// </summary>
        PUDS_ERROR_BUSHEAVY = 0x0000A,
        /// <summary>
        /// PCANTP Channel is in BUS-OFF error state
        /// </summary>
        PUDS_ERROR_BUSOFF = 0x0000B,
        /// <summary>
        /// Global CAN error, status code for composition of PCANBasic Errors.
        ///	Remove this value to get a PCAN-Basic TPCANStatus error code.
        /// </summary>
        PUDS_ERROR_CAN_ERROR = 0x80000000,
    }

    /// <summary>
    /// Represents network result values as defined in ISO 15765-2
    /// </summary>
    public enum TPUDSResult : byte
    {
        /// <summary>
        /// No network error
        /// </summary>	
        PUDS_RESULT_N_OK = 0x00,
        /// <summary>
        /// timeout occured between 2 frames transmission (sender and receiver side)
        /// </summary>
        PUDS_RESULT_N_TIMEOUT_A = 0x01,
        /// <summary>
        /// sender side timeout while waiting for flow control frame
        /// </summary>
        PUDS_RESULT_N_TIMEOUT_BS = 0x02,
        /// <summary>
        /// receiver side timeout while waiting for consecutive frame
        /// </summary>
        PUDS_RESULT_N_TIMEOUT_CR = 0x03,
        /// <summary>
        /// unexpected sequence number
        /// </summary>
        PUDS_RESULT_N_WRONG_SN = 0x04,
        /// <summary>
        /// invalid or unknown FlowStatus
        /// </summary>
        PUDS_RESULT_N_INVALID_FS = 0x05,
        /// <summary>
        /// unexpected protocol data unit
        /// </summary>
        PUDS_RESULT_N_UNEXP_PDU = 0x06,
        /// <summary>
        /// reception of flow control WAIT frame that exceeds the maximum counter defined by PUDS_PARAM_WFT_MAX
        /// </summary>
        PUDS_RESULT_N_WFT_OVRN = 0x07,
        /// <summary>
        /// buffer on the receiver side cannot store the data length (server side only)
        /// </summary>
        PUDS_RESULT_N_BUFFER_OVFLW = 0x08,
        /// <summary>
        /// general error
        /// </summary>
        PUDS_RESULT_N_ERROR = 0x09,
    }

    /// <summary>
    /// PCANTP parameters
    /// </summary>
    public enum TPUDSParameter : byte
    {
        /// <summary>
        /// 2 BYTE data describing the physical address of the equipment
        /// </summary>
        PUDS_PARAM_SERVER_ADDRESS = 0xC1,
        /// <summary>
        /// 2 BYTE data (2 BYTE functional address and MSB for status)
        /// describing a functional address to ignore or listen to
        /// </summary>
        PUDS_PARAM_SERVER_FILTER = 0xC2,
        /// <summary>
        /// 4 BYTE data describing the maximum time allowed by the client to transmit a request 
        /// See ISO-15765-3 §6.3.2 : /\P2Can_Req
        /// </summary>
        PUDS_PARAM_TIMEOUT_REQUEST = 0xC3,
        /// <summary>
        /// 4 BYTE data describing the maximum time allowed by the client to receive a response
        /// See ISO-15765-3 §6.3.2 : /\P2Can_Rsp
        /// </summary>
        PUDS_PARAM_TIMEOUT_RESPONSE = 0xC4,
        /// <summary>
        /// Require a pointer to a TPUDSSessionInfo structure
        /// </summary>
        PUDS_PARAM_SESSION_INFO = 0xC5,
        /// <summary>
        /// API version parameter
        /// </summary>
        PUDS_PARAM_API_VERSION = 0xC6,
        /// <summary>
        /// Define UDS receive-event handler, require a pointer to an event HANDLE. 
        /// </summary>
        PUDS_PARAM_RECEIVE_EVENT = 0xC7,
        /// <summary>
        /// Define a new ISO-TP mapping, requires a pointer to TPUDSMsg containing 
        /// the mapped CAN ID and CAN ID response in the DATA.RAW field.
        /// </summary>
        PUDS_PARAM_MAPPING_ADD = 0xC8,
        /// <summary>
        /// Remove an ISO-TP mapping, requires a pointer to TPUDSMsg containing the mapped CAN ID to remove. 
        /// </summary>
        PUDS_PARAM_MAPPING_REMOVE = 0xC9,

        /// <summary>
        /// 1 BYTE data describing the block size parameter (BS)
        /// </summary>
        PUDS_PARAM_BLOCK_SIZE = 0xE1,
        /// <summary>
        /// 1 BYTE data describing the seperation time parameter (STmin)
        /// </summary>
        PUDS_PARAM_SEPERATION_TIME = 0xE2,
        /// <summary>
        /// 1 BYTE data describing the debug mode 
        /// </summary>
        PUDS_PARAM_DEBUG = 0xE3,
        /// <summary>
        /// 1 Byte data describing the condition of a channel
        /// </summary>
        PUDS_PARAM_CHANNEL_CONDITION = 0xE4,
        /// <summary>
        /// Integer data describing the Wait Frame Transmissions parameter. 
        /// </summary>
        PUDS_PARAM_WFT_MAX = 0xE5,
        /// <summary>
        /// 1 BYTE data stating if CAN frame DLC uses padding or not
        /// </summary>
        PUDS_PARAM_CAN_DATA_PADDING = 0xE8,
        /// <summary>
        /// 1 BYTE data stating the value used for CAN data padding
        /// </summary>
        PUDS_PARAM_PADDING_VALUE = 0xED,
    }

    /// <summary>
    /// PUDS Service IDs defined in ISO 14229-1
    /// </summary>
    public enum TPUDSService : byte
    {
        PUDS_SI_DiagnosticSessionControl = 0x10,
        PUDS_SI_ECUReset = 0x11,
        PUDS_SI_SecurityAccess = 0x27,
        PUDS_SI_CommunicationControl = 0x28,
        PUDS_SI_TesterPresent = 0x3E,
        PUDS_SI_AccessTimingParameter = 0x83,
        PUDS_SI_SecuredDataTransmission = 0x84,
        PUDS_SI_ControlDTCSetting = 0x85,
        PUDS_SI_ResponseOnEvent = 0x86,
        PUDS_SI_LinkControl = 0x87,
        PUDS_SI_ReadDataByIdentifier = 0x22,
        PUDS_SI_ReadMemoryByAddress = 0x23,
        PUDS_SI_ReadScalingDataByIdentifier = 0x24,
        PUDS_SI_ReadDataByPeriodicIdentifier = 0x2A,
        PUDS_SI_DynamicallyDefineDataIdentifier = 0x2C,
        PUDS_SI_WriteDataByIdentifier = 0x2E,
        PUDS_SI_WriteMemoryByAddress = 0x3D,
        PUDS_SI_ClearDiagnosticInformation = 0x14,
        PUDS_SI_ReadDTCInformation = 0x19,
        PUDS_SI_InputOutputControlByIdentifier = 0x2F,
        PUDS_SI_RoutineControl = 0x31,
        PUDS_SI_RequestDownload = 0x34,
        PUDS_SI_RequestUpload = 0x35,
        PUDS_SI_TransferData = 0x36,
        PUDS_SI_RequestTransferExit = 0x37,
        /// <summary>
        /// Negative response code
        /// </summary>
        PUDS_NR_SI = 0x7f,
    }

    /// <summary>
    /// PUDS ISO_15765_4 address definitions
    /// </summary>
    public enum TPUDSAddress : byte
    {
        /// <summary>
        /// External test equipment
        /// </summary>
        PUDS_ISO_15765_4_ADDR_TEST_EQUIPMENT = 0xF1,
        /// <summary>
        /// OBD funtional system
        /// </summary>
        PUDS_ISO_15765_4_ADDR_OBD_FUNCTIONAL = 0x33,
        /// <summary>
        /// ECU 0
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_0 = 0x00,
        /// <summary>
        /// ECU 1
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_1 = 0x01,
        /// <summary>
        /// ECU 2
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_2 = 0x02,
        /// <summary>
        /// ECU 3
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_3 = 0x03,
        /// <summary>
        /// ECU 4
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_4 = 0x04,
        /// <summary>
        /// ECU 5
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_5 = 0x05,
        /// <summary>
        /// ECU 6
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_6 = 0x06,
        /// <summary>
        /// ECU 7
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_7 = 0x07,
        /// <summary>
        /// ECU 8
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_8 = 0x08,
        /// <summary>
        /// ECU 9
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_9 = 0x09,
        /// <summary>
        /// ECU 10
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_10 = 0x0A,
        /// <summary>
        /// ECU 11
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_11 = 0x0B,
        /// <summary>
        /// ECU 12
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_12 = 0x0C,
        /// <summary>
        /// ECU 13
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_13 = 0x0D,
        /// <summary>
        /// ECU 14
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_14 = 0x0E,
        /// <summary>
        /// ECU 15
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_15 = 0x0F,
        /// <summary>
        /// ECU 16
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_16 = 0x10,
        /// <summary>
        /// ECU 17
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_17 = 0x11,
        /// <summary>
        /// ECU 18
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_18 = 0x12,
        /// <summary>
        /// ECU 19
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_19 = 0x13,
        /// <summary>
        /// ECU 20
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_20 = 0x14,
        /// <summary>
        /// ECU 21
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_21 = 0x15,
        /// <summary>
        /// ECU 22
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_22 = 0x16,
        /// <summary>
        /// ECU 23
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_23 = 0x17,
        /// <summary>
        /// ECU 24
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_24 = 0x18,
        /// <summary>
        /// ECU 25
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_25 = 0x19,
        /// <summary>
        /// ECU 26
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_26 = 0x1A,
        /// <summary>
        /// ECU 27
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_27 = 0x1B,
        /// <summary>
        /// ECU 28
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_28 = 0x1C,
        /// <summary>
        /// ECU 29
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_29 = 0x1D,
        /// <summary>
        /// ECU 30
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_30 = 0x1E,
        /// <summary>
        /// ECU 31
        /// </summary>
        PUDS_ISO_15765_4_ADDR_ECU_31 = 0x1F,
    }

    /// <summary>
    /// PUDS ISO_15765_4 11 bit CAN Identifier
    /// </summary>
    public enum TPUDSCanId : uint
    {
        /// <summary>
        /// CAN ID for functionally addressed request messages sent by external test equipment
        /// </summary>        
        PUDS_ISO_15765_4_CAN_ID_FUNCTIONAL_REQUEST = 0x7DF,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #1
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_REQUEST_1 = 0x7E0,
        /// <summary>
        /// physical response CAN ID from ECU #1 to external test equipment
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_RESPONSE_1 = 0x7E8,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #2
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_REQUEST_2 = 0x7E1,
        /// <summary>
        /// physical response CAN ID from ECU #2 to external test equipment
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_RESPONSE_2 = 0x7E9,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #3
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_REQUEST_3 = 0x7E2,
        /// <summary>
        /// physical response CAN ID from ECU #3 to external test equipment
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_RESPONSE_3 = 0x7EA,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #4
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_REQUEST_4 = 0x7E3,
        /// <summary>
        /// physical response CAN ID from ECU #4 to external test equipment
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_RESPONSE_4 = 0x7EB,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #5
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_REQUEST_5 = 0x7E4,
        /// <summary>
        /// physical response CAN ID from ECU #5 to external test equipment
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_RESPONSE_5 = 0x7EC,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #6
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_REQUEST_6 = 0x7E5,
        /// <summary>
        /// physical response CAN ID from ECU #6 to external test equipment
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_RESPONSE_6 = 0x7ED,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #7
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_REQUEST_7 = 0x7E6,
        /// <summary>
        /// physical response CAN ID from ECU #7 to external test equipment
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_RESPONSE_7 = 0x7EE,
        /// <summary>
        /// physical request CAN ID from external test equipment to ECU #8
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_REQUEST_8 = 0x7E7,
        /// <summary>
        /// physical response CAN ID from ECU #8 to external test equipment
        /// </summary>
        PUDS_ISO_15765_4_CAN_ID_PHYSICAL_RESPONSE_8 = 0x7EF,
    }

    /// <summary>
    /// PUDS Protocol ISO-15765 definitions
    /// </summary>
    public enum TPUDSProtocol : byte
    {
        /// <summary>
        /// non ISO-TP frame (Unacknowledge Unsegmented Data Transfer)
        /// </summary>
	    PUDS_PROTOCOL_NONE = 0x00,
        /// <summary>
        /// using PCAN-ISO-TP with 11 BIT CAN ID, NORMAL addressing and diagnostic message
        /// </summary>
        PUDS_PROTOCOL_ISO_15765_2_11B = 0x01,
        /// <summary>
        /// using PCAN-ISO-TP with 11 BIT CAN ID, MIXED addressing and remote diagnostic message
        /// </summary>
        PUDS_PROTOCOL_ISO_15765_2_11B_REMOTE = 0x02,
        /// <summary>
        /// using PCAN-ISO-TP with 29 BIT CAN ID, FIXED NORMAL addressing and diagnostic message
        /// </summary>
        PUDS_PROTOCOL_ISO_15765_2_29B = 0x03,
        /// <summary>
        /// using PCAN-ISO-TP with 29 BIT CAN ID, MIXED addressing and remote diagnostic message
        /// </summary>
        PUDS_PROTOCOL_ISO_15765_2_29B_REMOTE = 0x04,
        /// <summary>
        /// using PCAN-ISO-TP with Enhanced diagnostics 29 bit CAN Identifiers
        /// </summary>
        PUDS_PROTOCOL_ISO_15765_3_29B = 0x05,
        /// <summary>
        /// using PCAN-ISO-TP with 29 BIT CAN ID, NORMAL addressing and diagnostic message
        /// Note: this protocol requires extra mapping definitions via PCAN-ISO-TP API
        /// </summary>
        PUDS_PROTOCOL_ISO_15765_2_29B_NORMAL = 0x06,
        /// <summary>
        /// using PCAN-ISO-TP with 11 BIT CAN ID, EXTENDED addressing and diagnostic message
        /// Note: this protocol requires extra mapping definitions via PCAN-ISO-TP API
        /// </summary>
        PUDS_PROTOCOL_ISO_15765_2_11B_EXTENDED = 0x07,
        /// <summary>
        /// using PCAN-ISO-TP with 29 BIT CAN ID, EXTENDED addressing and diagnostic message
        /// Note: this protocol requires extra mapping definitions via PCAN-ISO-TP API
        /// </summary>
        PUDS_PROTOCOL_ISO_15765_2_29B_EXTENDED = 0x08,
    }

    /// <summary>
    /// PUDS addressing type
    /// </summary>
    public enum TPUDSAddressingType : byte
    {
        /// <summary>
        /// Physical addressing
        /// </summary>
        PUDS_ADDRESSING_PHYSICAL = 0x01,
        /// <summary>
        /// Functional addressing
        /// </summary>
        PUDS_ADDRESSING_FUNCTIONAL = 0x02,
    }

    /// <summary>
    /// PCANTP message types
    /// </summary>
    public enum TPUDSMessageType : byte
    {
        /// <summary>
        /// UDS Request Message
        /// </summary>
        PUDS_MESSAGE_TYPE_REQUEST = 0x00,
        /// <summary>
        /// UDS Request/Response confirmation  Message
        /// </summary>
        PUDS_MESSAGE_TYPE_CONFIRM = 0x01,
        /// <summary>
        /// Incoming UDS Message
        /// </summary>
        PUDS_MESSAGE_TYPE_INDICATION = 0x02,
        /// <summary>
        /// UDS Message transmission started
        /// </summary>
        PUDS_MESSAGE_TYPE_INDICATION_TX = 0x03,
        /// <summary>
        /// Unacknowledge Unsegmented Data Transfert
        /// </summary>
        PUDS_MESSAGE_TYPE_CONFIRM_UUDT = 0x04,
    }

    /// <summary>
    /// PUDS Service Result
    /// </summary>
    public enum TPUDSServiceResult : byte
    {
        /// <summary>
        /// Response is valid and matches the requested Service ID.
        /// </summary>
        Confirmed = 0x00,
        /// <summary>
        /// Response is valid but an Negative Response Code was replied.
        /// </summary>
        NRC = 0x01,
        /// <summary>
        /// A network error occured in the ISO-TP layer.
        /// </summary>
        NetworkError = 0x02,
        /// <summary>
        /// Response does not match the requested UDS Service.
        /// </summary>
        ServiceMismatch = 0x03,
        /// <summary>
        /// Generic error, the message is not a valid response.
        /// </summary>
        GenericError = 0x04,
    }
    #endregion

    #region Structures
    /// <summary>
    /// PCAN-UDS Network Addressing Information
    /// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct TPUDSNetAddrInfo
    {
        /// <summary>
        /// Represents the origin of this message (address from 
        /// where this message was or will be sent)
        /// </summary>
        public byte SA;
        /// <summary>
        /// Represents the destination of this message (address to 
        /// where this message was or will be sent)
        /// </summary>
        public byte TA;
        /// <summary>
        /// Represents the kind of addressing being used for communication
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public TPUDSAddressingType TA_TYPE;
        /// <summary>
        /// Represents the destination of this message in a remote network 
        /// </summary>
        public byte RA;
        /// <summary>
        /// Represents the protocol being used for communication
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public TPUDSProtocol PROTOCOL;
    }

    /// <summary>
    /// PCAN-UDS Diagnostic Session Information of a server
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct TPUDSSessionInfo
    {
        /// <summary>
        /// Network address information
        /// </summary>
        public TPUDSNetAddrInfo NETADDRINFO;
        /// <summary>
        /// Activated Diagnostic Session (see PUDS_SVC_PARAM_DSC_xxx values)
        /// </summary>
	    public byte SESSION_TYPE;
        /// <summary>
        /// Default P2Can_Server_Max timing for the activated session
        /// </summary>
	    public ushort TIMEOUT_P2CAN_SERVER_MAX;
        /// <summary>
        /// Enhanced P2Can_Server_Max timing for the activated session
        /// </summary>
	    public ushort TIMEOUT_ENHANCED_P2CAN_SERVER_MAX;
    }

    /// <summary>
    /// PCAN-UDS Message
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct TPUDSMsg
    {
        /// <summary>
        /// Network Addressing Information
        /// </summary>
        public TPUDSNetAddrInfo NETADDRINFO;
        /// <summary>
        /// Result status of the network communication
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public TPUDSResult RESULT;
        /// <summary>
        /// States wether Positive Response Message should be suppressed.
        /// See constants PUDS_SUPPR_POS_RSP_MSG_INDICATION_BIT & PUDS_KEEP_POS_RSP_MSG_INDICATION_BIT
        /// </summary>
	    public byte NO_POSITIVE_RESPONSE_MSG;
        /// <summary>
        /// Data Length of the message
        /// </summary>
	    public ushort LEN;
        /// <summary>
        /// Type of UDS Message
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public TPUDSMessageType MSGTYPE;
        /// <summary>
        /// Represents the buffer containing the data of this message
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4095)]
        public byte[] DATA;


        /// <summary>
        /// Indicates if this message represents a valid Response
        /// </summary>
        [Obsolete("IsPositiveResponse is deprecated, please use IsValidResponse instead.")]
        public bool IsPositiveResponse
        {
            get
            {
                return IsValidResponse;
            }
        }
        /// <summary>
        /// Indicates if this message represents a valid Response
        /// </summary>
        public bool IsValidResponse
        {
            get
            {
                if (DATA != null)
                    return (DATA[0] & 0x40) == 0x40;
                return false;
            }
        }

        /// <summary>
        /// Indicates if this message represents a Negative-Response
        /// </summary>
        public bool IsNegativeResponse
        {
            get
            {
                if (DATA != null)
                    return DATA[0] == 0x7F;
                return false;
            }
        }

        /// <summary>
        /// Shows the data-byte representing the Service-ID of this message
        /// </summary>
        public byte ServiceID
        {
            get
            {
                if (DATA != null)
                    return IsNegativeResponse ? DATA[1] : DATA[0];
                return 0;
            }
        }

        /// <summary>
        /// Checks if a UDS message is a valid response to a request with the specified Service ID.
        /// </summary>
        /// <param name="serviceId">Service ID of the request.</param>
        /// <param name="nrc">If the response is valid but indicates a UDS error, this parameter will hold the UDS Negative Response Code.</param>
        /// <returns>Status indicating if the message is confirmed as a positive response.</returns>
        public TPUDSServiceResult CheckResponse(TPUDSService serviceId, out byte nrc)
        {
            nrc = 0;
            if (RESULT != TPUDSResult.PUDS_RESULT_N_OK)
                return TPUDSServiceResult.NetworkError;
            if (IsNegativeResponse)
            {
                if (DATA[1] != (byte)serviceId)
                    return TPUDSServiceResult.ServiceMismatch;
                nrc = DATA[2];
                return TPUDSServiceResult.NRC;
            }
            else if (IsValidResponse)
            {
                if (DATA[0] != ((byte)serviceId | 0x40))
                    return TPUDSServiceResult.ServiceMismatch;
                return TPUDSServiceResult.Confirmed;
            }
            return TPUDSServiceResult.GenericError;
        }
    }
    #endregion

    #region PCAN UDS Api
    public static class UDSApi
    {
        #region PCAN-BUS Handles Definition
        /// <summary>
        /// Undefined/default value for a PCAN bus
        /// </summary>
        public const TPUDSCANHandle PUDS_NONEBUS = 0x00;

        /// <summary>
        /// PCAN-ISA interface, channel 1
        /// </summary>
        public const TPUDSCANHandle PUDS_ISABUS1 = 0x21;
        /// <summary>
        /// PCAN-ISA interface, channel 2
        /// </summary>
        public const TPUDSCANHandle PUDS_ISABUS2 = 0x22;
        /// <summary>
        /// PCAN-ISA interface, channel 3
        /// </summary>
        public const TPUDSCANHandle PUDS_ISABUS3 = 0x23;
        /// <summary>
        /// PCAN-ISA interface, channel 4
        /// </summary>
        public const TPUDSCANHandle PUDS_ISABUS4 = 0x24;
        /// <summary>
        /// PCAN-ISA interface, channel 5
        /// </summary>
        public const TPUDSCANHandle PUDS_ISABUS5 = 0x25;
        /// <summary>
        /// PCAN-ISA interface, channel 6
        /// </summary>
        public const TPUDSCANHandle PUDS_ISABUS6 = 0x26;
        /// <summary>
        /// PCAN-ISA interface, channel 7
        /// </summary>
        public const TPUDSCANHandle PUDS_ISABUS7 = 0x27;
        /// <summary>
        /// PCAN-ISA interface, channel 8
        /// </summary>
        public const TPUDSCANHandle PUDS_ISABUS8 = 0x28;

        /// <summary>
        /// PPCAN-Dongle/LPT interface, channel 1 
        /// </summary>
        public const TPUDSCANHandle PUDS_DNGBUS1 = 0x31;

        /// <summary>
        /// PCAN-PCI interface, channel 1
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS1 = 0x41;
        /// <summary>
        /// PCAN-PCI interface, channel 2
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS2 = 0x42;
        /// <summary>
        /// PCAN-PCI interface, channel 3
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS3 = 0x43;
        /// <summary>
        /// PCAN-PCI interface, channel 4
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS4 = 0x44;
        /// <summary>
        /// PCAN-PCI interface, channel 5
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS5 = 0x45;
        /// <summary>
        /// PCAN-PCI interface, channel 6
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS6 = 0x46;
        /// <summary>
        /// PCAN-PCI interface, channel 7
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS7 = 0x47;
        /// <summary>
        /// PCAN-PCI interface, channel 8
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS8 = 0x48;
        /// <summary>
        /// PCAN-PCI interface, channel 9
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS9 = 0x409;
        /// <summary>
        /// PCAN-PCI interface, channel 10
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS10 = 0x40A;
        /// <summary>
        /// PCAN-PCI interface, channel 11
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS11 = 0x40B;
        /// <summary>
        /// PCAN-PCI interface, channel 12
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS12 = 0x40C;
        /// <summary>
        /// PCAN-PCI interface, channel 13
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS13 = 0x40D;
        /// <summary>
        /// PCAN-PCI interface, channel 14
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS14 = 0x40E;
        /// <summary>
        /// PCAN-PCI interface, channel 15
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS15 = 0x40F;
        /// <summary>
        /// PCAN-PCI interface, channel 16
        /// </summary>
        public const TPUDSCANHandle PUDS_PCIBUS16 = 0x410;

        /// <summary>
        /// PCAN-USB interface, channel 1
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS1 = 0x51;
        /// <summary>
        /// PCAN-USB interface, channel 2
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS2 = 0x52;
        /// <summary>
        /// PCAN-USB interface, channel 3
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS3 = 0x53;
        /// <summary>
        /// PCAN-USB interface, channel 4
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS4 = 0x54;
        /// <summary>
        /// PCAN-USB interface, channel 5
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS5 = 0x55;
        /// <summary>
        /// PCAN-USB interface, channel 6
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS6 = 0x56;
        /// <summary>
        /// PCAN-USB interface, channel 7
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS7 = 0x57;
        /// <summary>
        /// PCAN-USB interface, channel 8
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS8 = 0x58;
        /// <summary>
        /// PCAN-USB interface, channel 9
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS9 = 0x509;
        /// <summary>
        /// PCAN-USB interface, channel 10
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS10 = 0x50A;
        /// <summary>
        /// PCAN-USB interface, channel 11
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS11 = 0x50B;
        /// <summary>
        /// PCAN-USB interface, channel 12
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS12 = 0x50C;
        /// <summary>
        /// PCAN-USB interface, channel 13
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS13 = 0x50D;
        /// <summary>
        /// PCAN-USB interface, channel 14
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS14 = 0x50E;
        /// <summary>
        /// PCAN-USB interface, channel 15
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS15 = 0x50F;
        /// <summary>
        /// PCAN-USB interface, channel 16
        /// </summary>
        public const TPUDSCANHandle PUDS_USBBUS16 = 0x510;

        /// <summary>
        /// PCAN-PC Card interface, channel 1
        /// </summary>
        public const TPUDSCANHandle PUDS_PCCBUS1 = 0x61;
        /// <summary>
        /// PCAN-PC Card interface, channel 2
        /// </summary>
        public const TPUDSCANHandle PUDS_PCCBUS2 = 0x62;

        /// <summary>
        /// PCAN-LAN interface, channel 1
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS1 = 0x801;
        /// <summary>
        /// PCAN-LAN interface, channel 2
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS2 = 0x802;
        /// <summary>
        /// PCAN-LAN interface, channel 3
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS3 = 0x803;
        /// <summary>
        /// PCAN-LAN interface, channel 4
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS4 = 0x804;
        /// <summary>
        /// PCAN-LAN interface, channel 5
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS5 = 0x805;
        /// <summary>
        /// PCAN-LAN interface, channel 6
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS6 = 0x806;
        /// <summary>
        /// PCAN-LAN interface, channel 7
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS7 = 0x807;
        /// <summary>
        /// PCAN-LAN interface, channel 8
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS8 = 0x808;
        /// <summary>
        /// PCAN-LAN interface, channel 9
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS9 = 0x809;
        /// <summary>
        /// PCAN-LAN interface, channel 10
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS10 = 0x80A;
        /// <summary>
        /// PCAN-LAN interface, channel 11
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS11 = 0x80B;
        /// <summary>
        /// PCAN-LAN interface, channel 12
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS12 = 0x80C;
        /// <summary>
        /// PCAN-LAN interface, channel 13
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS13 = 0x80D;
        /// <summary>
        /// PCAN-LAN interface, channel 14
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS14 = 0x80E;
        /// <summary>
        /// PCAN-LAN interface, channel 15
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS15 = 0x80F;
        /// <summary>
        /// PCAN-LAN interface, channel 16
        /// </summary>
        public const TPUDSCANHandle PUDS_LANBUS16 = 0x810;
        #endregion

        #region Parameter values definition
        /// <summary>
        /// No debug messages
        /// </summary>
        public const byte PUDS_DEBUG_NONE = 0;
        /// <summary>
        /// Puts CAN debug messages to stdout
        /// </summary>
        public const byte PUDS_DEBUG_CAN = 1;
        /// <summary>
        /// The Channel is illegal or not available
        /// </summary>
        public const byte PUDS_CHANNEL_UNAVAILABLE = 0;
        /// <summary>
        /// The Channel is available
        /// </summary>
        public const byte PUDS_CHANNEL_AVAILABLE = 1;
        /// <summary>
        /// The Channel is valid, and is being used
        /// </summary>
        public const byte PUDS_CHANNEL_OCCUPIED = 2;

        /// <summary>
        /// Physical address for external test equipment
        /// </summary>
        public const byte PUDS_SERVER_ADDR_TEST_EQUIPMENT = 0xF1;
        /// <summary>
        /// Functional request address for Legislated OBD system
        /// </summary>
        public const byte PUDS_SERVER_ADDR_REQUEST_OBD_SYSTEM = 0x33;
        /// <summary>
        /// Flag stating that the address is defined as a ISO-15765-3 address
        /// </summary>
        public const ushort PUDS_SERVER_ADDR_FLAG_ENHANCED_ISO_15765_3 = 0x1000;
        /// <summary>
        /// Mask used for the ISO-15765-3 enhanced addresses
        /// </summary>
        public const ushort PUDS_SERVER_ADDR_MASK_ENHANCED_ISO_15765_3 = 0x07FF;
        /// <summary>
        /// Filter status : ignore (used to remove previously set filter)
        /// </summary>
        public const ushort PUDS_SERVER_FILTER_IGNORE = 0x0000;
        /// <summary>
        /// Filter status : listen to (must be OR'ed with the 2 BYTE functional address)
        /// </summary>
        public const ushort PUDS_SERVER_FILTER_LISTEN = 0x8000;
        /// <summary>
        /// Default maximum timeout for UDS transmit confirmation
        /// </summary>
        public const UInt32 PUDS_TIMEOUT_REQUEST = 10000;
        /// <summary>
        /// Default maximum timeout for UDS response reception
        /// </summary>
        public const UInt32 PUDS_TIMEOUT_RESPONSE = 10000;
        /// <summary>
        /// Default server performance requirement (See ISO-15765-3 §6.3.2)
        /// </summary>
        public const ushort PUDS_P2CAN_DEFAULT_SERVER_MAX = 50;
        /// <summary>
        /// Enhanced server performance requirement (See ISO-15765-3 §6.3.2)
        /// </summary>
        public const ushort PUDS_P2CAN_ENHANCED_SERVER_MAX = 5000;
        /// <summary>
        /// Uses CAN frame data optimization
        /// </summary>
        public const ushort PUDS_CAN_DATA_PADDING_NONE = 0x00;
        /// <summary>
        /// Uses CAN frame data padding (default, i.e. CAN DLC = 8)
        /// </summary>
        public const ushort PUDS_CAN_DATA_PADDING_ON = 0x01;
        /// <summary>
        /// Default value used if CAN data padding is enabled
        /// </summary>
        public const ushort PUDS_CAN_DATA_PADDING_VALUE = 0x55;
        #endregion

        #region Values definition related to UDS Message
        /// <summary>
        /// Maximum data length  of UDS messages
        /// </summary>
        public const ushort PUDS_MAX_DATA = 4095;
        /// <summary>
        /// Value (for member NO_POSITIVE_RESPONSE_MSG) stating to suppress positive response messages
        /// </summary>
        public const byte PUDS_SUPPR_POS_RSP_MSG_INDICATION_BIT = 0x80;
        /// <summary>
        /// Default Value (for member NO_POSITIVE_RESPONSE_MSG) stating to keep positive response messages
        /// </summary>
        public const byte PUDS_KEEP_POS_RSP_MSG_INDICATION_BIT = 0x00;
        /// <summary>
        /// Negative response code: Server wants more time
        /// </summary>
        public const byte PUDS_NRC_EXTENDED_TIMING = 0x78;
        /// <summary>
        /// Positive response offset
        /// </summary>
        public const byte PUDS_SI_POSITIVE_RESPONSE = 0x40;
        #endregion

        #region PCAN UDS API Implementation
        /// <summary>
        /// Initializes a PUDS-Client based on a PUDS-Channel
        /// </summary>
        /// <remarks>Only one UDS-Client can be initialized per CAN-Channel</remarks>
        /// <param name="CanChannel">The PCAN-Basic channel to be used as UDS client</param>
        /// <param name="Baudrate">The CAN Hardware speed</param>
        /// <param name="HwType">NON PLUG&PLAY: The type of hardware and operation mode</param>
        /// <param name="IOPort">NON PLUG&PLAY: The I/O address for the parallel port</param>
        /// <param name="Interrupt">NON PLUG&PLAY: Interrupt number of the parallel port</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Initialize")]
        public static extern TPUDSStatus Initialize(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [MarshalAs(UnmanagedType.U2)]
            TPUDSBaudrate Baudrate,
            [MarshalAs(UnmanagedType.U1)]
            TPUDSHWType HwType,
            UInt32 IOPort,
            UInt16 Interrupt);

        /// <summary>
        /// Initializes a PUDS-Client based on a PUDS-Channel
        /// </summary>
        /// <remarks>Only one UDS-Client can be initialized per CAN-Channel</remarks>
        /// <param name="CanChannel">The PCAN-Basic channel to be used as UDS client</param>
        /// <param name="Baudrate">The CAN Hardware speed</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        public static TPUDSStatus Initialize(
            TPUDSCANHandle CanChannel,
            TPUDSBaudrate Baudrate)
        {
            return Initialize(CanChannel, Baudrate, (TPUDSHWType)0, 0, 0);
        }

        /// <summary>
        /// Uninitializes a PUDS-Client initialized before
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Uninitialize")]
        public static extern TPUDSStatus Uninitialize(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel);

        /// <summary>
        /// Resets the receive and transmit queues of a PUDS-Client 
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Reset")]
        public static extern TPUDSStatus Reset(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel);

        /// <summary>
        /// Gets information about the internal BUS status of a PUDS CAN-Channel.
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetStatus")]
        public static extern TPUDSStatus GetStatus(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel);

        /// <summary>
        /// Reads a PUDS message from the receive queue of a PUDS-Client
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <param name="MessageBuffer">A TPUDSMsg structure buffer to store the PUDS message</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Read")]
        public static extern TPUDSStatus Read(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            out TPUDSMsg MessageBuffer);

        /// <summary>
        /// Transmits a PUDS message
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <param name="MessageBuffer">A TPUDSMsg buffer with the message to be sent</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_Write")]
        public static extern TPUDSStatus Write(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer);


        /// <summary>
        /// Retrieves a PUDS-Client value
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel Handle representing a PUDS-Client</param>
        /// <param name="Parameter">The TPUDSParameter parameter to get</param>
        /// <param name="StringBuffer">Buffer for the parameter value</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetValue")]
        public static extern TPUDSStatus GetValue(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [MarshalAs(UnmanagedType.U1)]
            TPUDSParameter Parameter,
            StringBuilder StringBuffer,
            UInt32 BufferLength);
        /// <summary>
        /// Retrieves a PUDS-Client value
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel Handle representing a PUDS-Client</param>
        /// <param name="Parameter">The TPUDSParameter parameter to get</param>
        /// <param name="NumericBuffer">Buffer for the parameter value</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetValue")]
        public static extern TPUDSStatus GetValue(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [MarshalAs(UnmanagedType.U1)]
            TPUDSParameter Parameter,
            out UInt32 NumericBuffer,
            UInt32 BufferLength);
        /// <summary>
        /// Retrieves a PUDS-Client value
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel Handle representing a PUDS-Client</param>
        /// <param name="Parameter">The TPUDSParameter parameter to get</param>
        /// <param name="Buffer">Buffer for the parameter value</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetValue")]
        public static extern TPUDSStatus GetValue(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [MarshalAs(UnmanagedType.U1)]
            TPUDSParameter Parameter,
            [MarshalAs(UnmanagedType.LPArray)]
            [Out] Byte[] Buffer,
            UInt32 BufferLength);
        /// <summary>
        /// Retrieves a PUDS-Client value
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel Handle representing a PUDS-Client</param>
        /// <param name="Parameter">The TPUDSParameter parameter to get</param>
        /// <param name="Buffer">Buffer for the parameter value</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_GetValue")]
        public static extern TPUDSStatus GetValue(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [MarshalAs(UnmanagedType.U1)]
            TPUDSParameter Parameter,
            IntPtr Buffer,
            UInt32 BufferLength);

        /// <summary>
        /// Configures or sets a PUDS-Client value
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel Handle representing a PUDS-Client</param>
        /// <param name="Parameter">The TPUDSParameter parameter to set</param>
        /// <param name="NumericBuffer">Buffer with the value to be set</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SetValue")]
        public static extern TPUDSStatus SetValue(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [MarshalAs(UnmanagedType.U1)]
            TPUDSParameter Parameter,
            ref UInt32 NumericBuffer,
            UInt32 BufferLength);
        /// <summary>
        /// Configures or sets a PUDS-Client value
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel Handle representing a PUDS-Client</param>
        /// <param name="Parameter">The TPUDSParameter parameter to set</param>
        /// <param name="StringBuffer">Buffer with the value to be set</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SetValue")]
        public static extern TPUDSStatus SetValue(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [MarshalAs(UnmanagedType.U1)]
            TPUDSParameter Parameter,
            [MarshalAs(UnmanagedType.LPStr, SizeParamIndex = 3)]
            string StringBuffer,
            UInt32 BufferLength);
        /// <summary>
        /// Configures or sets a PUDS-Client value
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel Handle representing a PUDS-Client</param>
        /// <param name="Parameter">The TPUDSParameter parameter to set</param>
        /// <param name="Buffer">Buffer with the value to be set</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SetValue")]
        public static extern TPUDSStatus SetValue(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [MarshalAs(UnmanagedType.U1)]
            TPUDSParameter Parameter,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            Byte[] Buffer,
            UInt32 BufferLength);
        /// <summary>
        /// Configures or sets a PUDS-Client value
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel Handle representing a PUDS-Client</param>
        /// <param name="Parameter">The TPUDSParameter parameter to set</param>
        /// <param name="Buffer">Buffer with the value to be set</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SetValue")]
        public static extern TPUDSStatus SetValue(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [MarshalAs(UnmanagedType.U1)]
            TPUDSParameter Parameter,
            IntPtr Buffer,
            UInt32 BufferLength);
        #endregion

        #region PCAN UDS API Implementation : Service handlers
        /// <summary>
        /// Waits for a message (a response or a transmit confirmation) based on a UDS Message Request.
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <param name="MessageBuffer">A TPUDSMsg structure buffer to store the PUDS response</param>
        /// <param name="MessageRequest">A sent TPUDSMsg message</param>
        /// <param name="IsWaitForTransmit">The message to wait for is a Transmit Confirmation or not</param>
        /// <param name="TimeInterval">Time interval to check for new message</param>
        /// <param name="Timeout">Maximum time to wait for the message</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_WaitForSingleMessage")]
        public static extern TPUDSStatus WaitForSingleMessage(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            out TPUDSMsg MessageBuffer,
            ref TPUDSMsg MessageRequest,
            bool IsWaitForTransmit,
            UInt32 TimeInterval,
            UInt32 Timeout);

        /// <summary>
        /// Waits for multiple messages (multiple responses from a functional request for instance) based on a UDS Message Request.
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <param name="Buffer">Buffer must be an array of 'MaxCount' entries (must have at least 
        /// a size of iMaxCount * sizeof(TPUDSMsg) bytes</param>
        /// <param name="MaxCount">Size of the Buffer array (max. messages that can be received)</param>
        /// <param name="pCount">Buffer for the real number of messages read</param>
        /// <param name="MessageRequest">A sent TPUDSMsg message</param>
        /// <param name="TimeInterval">Time interval to check for new message</param>
        /// <param name="Timeout">Maximum time to wait for the message</param>
        /// <param name="TimeoutEnhanced">Maximum time to wait for the message in UDS Enhanced mode</param>
        /// <param name="WaitUntilTimeout">if <code>FALSE</code> the function is interrupted if pCount reaches MaxCount.</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success, 
        ///	PUDS_ERROR_OVERFLOW indicates success but Buffer was too small to hold all responses.</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_WaitForMultipleMessage")]
        public static extern TPUDSStatus WaitForMultipleMessage(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [In, Out]
            TPUDSMsg[] Buffer,
            UInt32 MaxCount,
            out UInt32 pCount,
            ref TPUDSMsg MessageRequest,
            UInt32 TimeInterval,
            UInt32 Timeout,
            UInt32 TimeoutEnhanced,
            bool WaitUntilTimeout);

        /// <summary>
        /// Handles the communication workflow for a UDS service expecting a single response.
        /// </summary>
        ///	<remark>
        ///	The function waits for a transmit confirmation then for a message response.
        ///	Even if the SuppressPositiveResponseMessage flag is set, the function will still wait 
        /// for an eventual Negative Response.
        ///	</remark>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <param name="MessageBuffer">A TPUDSMsg structure buffer to store the PUDS response</param>
        /// <param name="MessageRequest">A sent TPUDSMsg message</param>
        /// <param name="MessageReqBuffer">A TPUDSMsg structure buffer to store the PUDS request confirmation 
        ///	(if <code>NULL</code>, the result confirmation will be set in MessageRequest parameter)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_WaitForService")]
        public static extern TPUDSStatus WaitForService(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            out TPUDSMsg MessageBuffer,
            ref TPUDSMsg MessageRequest,
            out TPUDSMsg MessageReqBuffer);

        /// <summary>
        /// Handles the communication workflow for a UDS service expecting multiple responses.
        /// </summary>
        ///	<remark>
        ///	The function waits for a transmit confirmation then for N message responses.
        ///	Even if the SuppressPositiveResponseMessage flag is set, the function will still wait 
        /// for eventual Negative Responses.
        ///	</remark>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <param name="Buffer">Buffer must be an array of 'MaxCount' entries (must have at least 
        /// a size of iMaxCount * sizeof(TPUDSMsg) bytes</param>
        /// <param name="MaxCount">Size of the Buffer array (max. messages that can be received)</param>
        /// <param name="pCount">Buffer for the real number of messages read</param>
        /// <param name="WaitUntilTimeout">if <code>FALSE</code> the function is interrupted if pCount reaches MaxCount.</param>
        /// <param name="MessageRequest">A sent TPUDSMsg message</param>
        /// <param name="MessageReqBuffer">A TPUDSMsg structure buffer to store the PUDS request confirmation 
        ///	(if <code>NULL</code>, the result confirmation will be set in MessageRequest parameter)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success, 
        ///	PUDS_ERROR_OVERFLOW indicates success but Buffer was too small to hold all responses.</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_WaitForServiceFunctional")]
        public static extern TPUDSStatus WaitForServiceFunctional(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            [In, Out]
            TPUDSMsg[] Buffer,
            UInt32 MaxCount,
            out UInt32 pCount,
            bool WaitUntilTimeout,
            ref TPUDSMsg MessageRequest,
            out TPUDSMsg MessageReqBuffer);

        /// <summary>
        /// Process a UDS response message to manage ISO-14229/15765 features (like session handling).
        /// </summary>
        /// <param name="CanChannel">A PUDS CAN-Channel representing a PUDS-Client</param>
        /// <param name="MessageBuffer">A TPUDSMsg structure buffer representing the PUDS Response Message</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_ProcessResponse")]
        public static extern TPUDSStatus ProcessResponse(
            [MarshalAs(UnmanagedType.U2)]
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer);
        #endregion

        #region PCAN UDS API Implementation : Services

        #region UDS Service: DiagnosticSessionControl
        // ISO-15765-3:2004 §9.2.1 p.42 & ISO-14229-1:2006 §9.2 p.36

        /// <summary>
        /// Subfunction parameter for UDS service DiagnosticSessionControl
        /// </summary>
        public enum TPUDSSvcParamDSC : byte
        {
            /// <summary>
            /// Default Session
            /// </summary>
            PUDS_SVC_PARAM_DSC_DS = 0x01,
            /// <summary>
            /// ECU Programming Session
            /// </summary>
            PUDS_SVC_PARAM_DSC_ECUPS = 0x02,
            /// <summary>
            /// ECU Extended Diagnostic Session
            /// </summary>
            PUDS_SVC_PARAM_DSC_ECUEDS = 0x03,
            /// <summary>
            /// Safety System Diagnostic Session
            /// </summary>
            PUDS_SVC_PARAM_DSC_SSDS = 0x04
        }

        /// <summary>
        /// The DiagnosticSessionControl service is used to enable different diagnostic sessions in the server.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="SessionType">Subfunction parameter: type of the session (see PUDS_SVC_PARAM_DSC_xxx)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcDiagnosticSessionControl")]
        public static extern TPUDSStatus SvcDiagnosticSessionControl(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamDSC SessionType);

        #endregion

        #region UDS Service: ECUReset
        // ISO-15765-3:2004 §9.2.2 p.42 && ISO-14229-1:2006 §9.3 p.42

        /// <summary>
        /// Subfunction parameter for UDS service ECURest
        /// </summary>
        public enum TPUDSSvcParamER : byte
        {
            /// <summary>
            /// Hard Reset
            /// </summary>
            PUDS_SVC_PARAM_ER_HR = 0x01,
            /// <summary>
            /// Key Off on Reset
            /// </summary>
            PUDS_SVC_PARAM_ER_KOFFONR = 0x02,
            /// <summary>
            /// Soft Reset
            /// </summary>
            PUDS_SVC_PARAM_ER_SR = 0x03,
            /// <summary>
            /// Enable Rapid Power Shutdown
            /// </summary>
            PUDS_SVC_PARAM_ER_ERPSD = 0x04,
            /// <summary>
            /// Disable Rapid Power Shutdown
            /// </summary>
            PUDS_SVC_PARAM_ER_DRPSD = 0x05,
        }
        /// <summary>
        /// The ECUReset service is used by the client to request a server reset.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="ResetType">Subfunction parameter: type of Reset (see PUDS_SVC_PARAM_ER_xxx)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcECUReset")]
        public static extern TPUDSStatus SvcECUReset(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamER ResetType);
        #endregion

        #region UDS Service: SecurityAccess
        // ISO-15765-3:2004 §9.2.3 p.43 && ISO-14229-1:2006 §9.4 p.45

        /// <summary>
        /// SecurityAccessType : Request Seed and Send Key values
        /// </summary>        
        public const byte PUDS_SVC_PARAM_SA_RSD_1 = 0x01;	// Request Seed
        public const byte PUDS_SVC_PARAM_SA_RSD_3 = 0x03;	// Request Seed
        public const byte PUDS_SVC_PARAM_SA_RSD_5 = 0x05;	// Request Seed
        public const byte PUDS_SVC_PARAM_SA_RSD_MIN = 0x07;	// Request Seed (odd numbers)
        public const byte PUDS_SVC_PARAM_SA_RSD_MAX = 0x5F;	// Request Seed (odd numbers)
        public const byte PUDS_SVC_PARAM_SA_SK_2 = 0x02;	// Send Key
        public const byte PUDS_SVC_PARAM_SA_SK_4 = 0x04;	// Send Key
        public const byte PUDS_SVC_PARAM_SA_SK_6 = 0x06;	// Send Key
        public const byte PUDS_SVC_PARAM_SA_SK_MIN = 0x08;	// Send Key (even numbers)
        public const byte PUDS_SVC_PARAM_SA_SK_MAX = 0x60;	// Send Key (even numbers)

        /// <summary>
        /// SecurityAccess service provides a means to access data and/or diagnostic services which have
        ///	restricted access for security, emissions or safety reasons.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="SecurityAccessType">Subfunction parameter: type of SecurityAccess (see PUDS_SVC_PARAM_SA_xxx)</param>
        /// <param name="Buffer">If Requesting Seed, buffer is the optional data to transmit to a server (like identification).
        ///	If Sending Key, data holds the value generated by the security algorithm corresponding to a specific “seed” value</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcSecurityAccess")]
        public static extern TPUDSStatus SvcSecurityAccess(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            byte SecurityAccessType,
            byte[] Buffer,
            ushort BufferLength);
        #endregion

        #region UDS Service: CommunicationControl
        // ISO-15765-3:2004 §9.2.4 p.43 && ISO-14229-1:2006 §9.5 p.52

        /// <summary>
        /// ControlType: Subfunction parameter for UDS service CommunicationControl 
        /// </summary>
        public enum TPUDSSvcParamCC : byte
        {
            /// <summary>
            /// Enable Rx and Tx
            /// </summary>
            PUDS_SVC_PARAM_CC_ERXTX = 0x00,
            /// <summary>
            /// Enable Rx and Disable Tx
            /// </summary>
            PUDS_SVC_PARAM_CC_ERXDTX = 0x01,
            /// <summary>
            /// Disable Rx and Enable Tx
            /// </summary>
            PUDS_SVC_PARAM_CC_DRXETX = 0x02,
            /// <summary>
            /// Disable Rx and Tx
            /// </summary>
            PUDS_SVC_PARAM_CC_DRXTX = 0x03,
        }

        /// <summary>
        /// CommunicationType Flag: Application (01b)
        /// </summary>  
        public const byte PUDS_SVC_PARAM_CC_FLAG_APPL = 0x01;
        /// <summary>
        /// CommunicationType Flag: NetworkManagement (10b)
        /// </summary>
        public const byte PUDS_SVC_PARAM_CC_FLAG_NWM = 0x02;
        /// <summary>
        /// CommunicationType Flag: Disable/Enable specified communicationType (see Flags APPL/NMW)
        /// </summary>
        public const byte PUDS_SVC_PARAM_CC_FLAG_DESCTIRNCN = 0x00;
        // in the receiving node and all connected networks
        /// <summary>
        /// CommunicationType Flag: Disable/Enable network which request is received on
        /// </summary>
        public const byte PUDS_SVC_PARAM_CC_FLAG_DENWRIRO = 0xF0;
        /// <summary>
        /// CommunicationType Flag: Disable/Enable specific network identified by network number (minimum value)
        /// </summary>
        public const byte PUDS_SVC_PARAM_CC_FLAG_DESNIBNN_MIN = 0x10;
        /// <summary>
        /// CommunicationType Flag: Disable/Enable specific network identified by network number (maximum value)
        /// </summary>
        public const byte PUDS_SVC_PARAM_CC_FLAG_DESNIBNN_MAX = 0xE0;
        /// <summary>
        /// CommunicationType Flag: Mask for DESNIBNN bits
        /// </summary>
        public const byte PUDS_SVC_PARAM_CC_FLAG_DESNIBNN_MASK = 0xF0;

        /// <summary>
        ///	CommunicationControl service's purpose is to switch on/off the transmission 
        ///	and/or the reception of certain messages of (a) server(s).
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="ControlType">Subfunction parameter: type of CommunicationControl (see PUDS_SVC_PARAM_CC_xxx)</param>
        /// <param name="CommunicationType">a bit-code value to reference the kind of communication to be controlled,
        ///	See PUDS_SVC_PARAM_CC_FLAG_xxx flags and ISO_14229-2006 §B.1 for bit-encoding</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcCommunicationControl")]
        public static extern TPUDSStatus SvcCommunicationControl(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamCC ControlType,
            byte CommunicationType);
        #endregion

        #region UDS Service: TesterPresent
        // ISO-15765-3:2004 §9.2.5 p.43 && ISO-14229-1:2006 §9.6 p.55

        /// <summary>
        /// TesterPresentType: Subfunction parameter for UDS service TesterPresent
        /// </summary>
        public enum TPUDSSvcParamTP : byte
        {
            /// <summary>
            /// Zero SubFunction
            /// </summary>
            PUDS_SVC_PARAM_TP_ZSUBF = 0x00,
        }

        /// <summary>
        ///	TesterPresent service indicates to a server (or servers) that a client is still connected
        ///	to the vehicle and that certain diagnostic services and/or communications 
        ///	that have been previously activated are to remain active.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="TesterPresentType">No Subfunction parameter by default (PUDS_SVC_PARAM_TP_ZSUBF)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcTesterPresent")]
        public static extern TPUDSStatus SvcTesterPresent(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamTP TesterPresentType);

        /// <summary>
        ///	TesterPresent service indicates to a server (or servers) that a client is still connected
        ///	to the vehicle and that certain diagnostic services and/or communications 
        ///	that have been previously activated are to remain active.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        public static TPUDSStatus SvcTesterPresent(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer)
        {
            return SvcTesterPresent(CanChannel, ref MessageBuffer, TPUDSSvcParamTP.PUDS_SVC_PARAM_TP_ZSUBF);
        }
        #endregion

        #region UDS Service: SecuredDataTransmission
        // ISO-15765-3:2004 §9.2.6 p.44 && ISO-14229-1:2006 §9.8 p.63

        /// <summary>
        ///	SecuredDataTransmission service's purpose is to transmit data that is protected 
        ///	against attacks from third parties, which could endanger data security.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="Buffer">buffer containing the data as processed by the Security Sub-Layer (See ISO-15764)</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcSecuredDataTransmission")]
        public static extern TPUDSStatus SvcSecuredDataTransmission(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            byte[] Buffer,
            ushort BufferLength);
        #endregion

        #region UDS Service: ControlDTCSetting
        // ISO-15765-3:2004 §9.2.7 p.44 && ISO-14229-1:2006 §9.9 p.69

        /// <summary>
        /// DTCSettingType: Subfunction parameter for UDS service ControlDTCSetting
        /// ISO
        /// </summary>
        public enum TPUDSSvcParamCDTCS : byte
        {
            /// <summary>
            /// The server(s) shall resume the setting of diagnostic trouble codes
            /// </summary>
            PUDS_SVC_PARAM_CDTCS_ON = 0x01,
            /// <summary>
            /// The server(s) shall stop the setting of diagnostic trouble codes
            /// </summary>
            PUDS_SVC_PARAM_CDTCS_OFF = 0x02,
        }

        /// <summary>
        ///	ControlDTCSetting service shall be used by a client to stop or resume the setting of 
        ///	diagnostic trouble codes (DTCs) in the server(s).
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="DTCSettingType">Subfunction parameter (see PUDS_SVC_PARAM_CDTCS_xxx)</param>
        /// <param name="Buffer">This parameter record is user-optional and transmits data to a server when controlling the DTC setting. 
        ///	It can contain a list of DTCs to be turned on or off.</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcControlDTCSetting")]
        public static extern TPUDSStatus SvcControlDTCSetting(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamCDTCS DTCSettingType,
            byte[] Buffer,
            ushort BufferLength);
        #endregion

        #region UDS Service: ResponseOnEvent
        // ISO-15765-3:2004 §9.2.8 p.44 && ISO-14229-1:2006 §9.10 p.73

        /// <summary>
        /// EventType: Subfunction parameter for UDS service ResponseOnEvent
        /// </summary>
        public enum TPUDSSvcParamROE : byte
        {
            /// <summary>
            /// Stop Response On Event
            /// </summary>
            PUDS_SVC_PARAM_ROE_STPROE = 0x00,
            /// <summary>
            /// On DTC Status Change
            /// </summary>
            PUDS_SVC_PARAM_ROE_ONDTCS = 0x01,
            /// <summary>
            /// On Timer Interrupt
            /// </summary>
            PUDS_SVC_PARAM_ROE_OTI = 0x02,
            /// <summary>
            /// On Change Of Data Identifier
            /// </summary>
            PUDS_SVC_PARAM_ROE_OCODID = 0x03,
            /// <summary>
            /// Report Activated Events
            /// </summary>
            PUDS_SVC_PARAM_ROE_RAE = 0x04,
            /// <summary>
            /// Start Response On Event
            /// </summary>
            PUDS_SVC_PARAM_ROE_STRTROE = 0x05,
            /// <summary>
            /// Clear Response On Event
            /// </summary>
            PUDS_SVC_PARAM_ROE_CLRROE = 0x06,
            /// <summary>
            /// On Comparison Of Values
            /// </summary>
            PUDS_SVC_PARAM_ROE_OCOV = 0x07,
        }

        /// <summary>
        /// RoE Recommended service (first byte of ServiceToRespondTo Record)
        /// </summary>
        public enum TPUDSSvcParamROERecommendedServiceID : byte
        {
            PUDS_SVC_PARAM_ROE_STRT_SI_RDBI = TPUDSService.PUDS_SI_ReadDataByIdentifier,
            PUDS_SVC_PARAM_ROE_STRT_SI_RDTCI = TPUDSService.PUDS_SI_ReadDTCInformation,
            PUDS_SVC_PARAM_ROE_STRT_SI_RC = TPUDSService.PUDS_SI_RoutineControl,
            PUDS_SVC_PARAM_ROE_STRT_SI_IOCBI = TPUDSService.PUDS_SI_InputOutputControlByIdentifier,
        }

        /// <summary>
        /// expected size of EventTypeRecord for ROE_STPROE
        /// </summary>
        public const byte PUDS_SVC_PARAM_ROE_STPROE_LEN = 0;
        /// <summary>
        /// expected size of EventTypeRecord for ROE_ONDTCS
        /// </summary>
        public const byte PUDS_SVC_PARAM_ROE_ONDTCS_LEN = 1;
        /// <summary>
        /// expected size of EventTypeRecord for ROE_OTI
        /// </summary>
        public const byte PUDS_SVC_PARAM_ROE_OTI_LEN = 1;
        /// <summary>
        /// expected size of EventTypeRecord for ROE_OCODID
        /// </summary>
        public const byte PUDS_SVC_PARAM_ROE_OCODID_LEN = 2;
        /// <summary>
        /// expected size of EventTypeRecord for ROE_RAE
        /// </summary>
        public const byte PUDS_SVC_PARAM_ROE_RAE_LEN = 0;
        /// <summary>
        /// expected size of EventTypeRecord for ROE_STRTROE
        /// </summary>
        public const byte PUDS_SVC_PARAM_ROE_STRTROE_LEN = 0;
        /// <summary>
        /// expected size of EventTypeRecord for ROE_CLRROE
        /// </summary>
        public const byte PUDS_SVC_PARAM_ROE_CLRROE_LEN = 0;
        /// <summary>
        /// expected size of EventTypeRecord for ROE_OCOV
        /// </summary>
        public const byte PUDS_SVC_PARAM_ROE_OCOV_LEN = 10;
        /// <summary>
        /// Infinite Time To Response (eventWindowTime parameter)
        /// </summary>
        public const byte PUDS_SVC_PARAM_ROE_EWT_ITTR = 0x02;

        /// <summary>
        ///	The ResponseOnEvent service requests a server to 
        ///	start or stop transmission of responses on a specified event.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="EventType">Subfunction parameter: event type (see PUDS_SVC_PARAM_ROE_xxx)</param>
        /// <param name="StoreEvent">Storage State (TRUE = Store Event, FALSE = Do Not Store Event)</param>
        /// <param name="EventWindowTime">Specify a window for the event logic to be active in the server (see PUDS_SVC_PARAM_ROE_EWT_ITTR)</param>
        /// <param name="EventTypeRecord">Additional parameters for the specified eventType</param>
        /// <param name="EventTypeRecordLength">Size in bytes of the EventType Record (see PUDS_SVC_PARAM_ROE_xxx_LEN)</param>
        /// <param name="ServiceToRespondToRecord">Service parameters, with first byte as service Id (see PUDS_SVC_PARAM_ROE_STRT_SI_xxx)</param>
        /// <param name="ServiceToRespondToRecordLength">Size in bytes of the ServiceToRespondTo Record</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcResponseOnEvent")]
        public static extern TPUDSStatus SvcResponseOnEvent(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamROE EventType,
            bool StoreEvent,
            byte EventWindowTime,
            byte[] EventTypeRecord,
            ushort EventTypeRecordLength,
            byte[] ServiceToRespondToRecord,
            ushort ServiceToRespondToRecordLength);
        #endregion

        #region UDS Service: LinkControl
        // ISO-15765-3:2004 §9.2.9 p.47 && ISO-14229-1:2006 §9.11 p.91

        /// <summary>
        /// LinkControlType: Subfunction parameter for UDS service LinkControl
        /// </summary>
        public enum TPUDSSvcParamLC : byte
        {
            /// <summary>
            /// Verify Baudrate Transition With Fixed Baudrate
            /// </summary>
            PUDS_SVC_PARAM_LC_VBTWFBR = 0x01,
            /// <summary>
            /// Verify Baudrate Transition With Specific Baudrate
            /// </summary>
            PUDS_SVC_PARAM_LC_VBTWSBR = 0x02,
            /// <summary>
            /// Transition Baudrate
            /// </summary>
            PUDS_SVC_PARAM_LC_TB = 0x03,
        }

        /// <summary>
        /// BaudrateIdentifier: standard Baudrate Identifiers
        /// </summary>
        public enum TPUDSSvcParamLCBaudrateIdentifier : byte
        {
            /// <summary>
            /// standard PC baud rate of 9.6 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_9600 = 0x01,
            /// <summary>
            /// standard PC baud rate of 19.2 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_19200 = 0x02,
            /// <summary>
            /// standard PC baud rate of 38.4 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_38400 = 0x03,
            /// <summary>
            /// standard PC baud rate of 57.6 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_57600 = 0x04,
            /// <summary>
            /// standard PC baud rate of 115.2 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_PC_115200 = 0x05,
            /// <summary>
            /// standard CAN baud rate of 125 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_CAN_125K = 0x10,
            /// <summary>
            /// standard CAN baud rate of 250 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_CAN_250K = 0x11,
            /// <summary>
            /// standard CAN baud rate of 500 KBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_CAN_500K = 0x12,
            /// <summary>
            /// standard CAN baud rate of 1 MBaud
            /// </summary>
            PUDS_SVC_PARAM_LC_BAUDRATE_CAN_1M = 0x13,
        }

        /// <summary>
        ///	The LinkControl service is used to control the communication link baud rate
        ///	between the client and the server(s) for the exchange of diagnostic data.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="LinkControlType">Subfunction parameter: Link Control Type (see PUDS_SVC_PARAM_LC_xxx)</param>
        /// <param name="BaudrateIdentifier">defined baud rate identifier (see PUDS_SVC_PARAM_LC_BAUDRATE_xxx)</param>
        /// <param name="LinkBaudrate">used only with PUDS_SVC_PARAM_LC_VBTWSBR parameter: 
        ///	a three-byte value baud rate (baudrate High, Middle and Low Bytes).
        ///	</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success </returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcLinkControl")]
        public static extern TPUDSStatus SvcLinkControl(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamLC LinkControlType,
            byte BaudrateIdentifier,
            UInt32 LinkBaudrate);

        /// <summary>
        ///	The LinkControl service is used to control the communication link baud rate
        ///	between the client and the server(s) for the exchange of diagnostic data.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="LinkControlType">Subfunction parameter: Link Control Type (see PUDS_SVC_PARAM_LC_xxx)</param>
        /// <param name="BaudrateIdentifier">defined baud rate identifier (see PUDS_SVC_PARAM_LC_BAUDRATE_xxx)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success </returns>
        public static TPUDSStatus SvcLinkControl(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamLC LinkControlType,
            byte BaudrateIdentifier)
        {
            return SvcLinkControl(CanChannel, ref MessageBuffer, LinkControlType, BaudrateIdentifier, 0);
        }
        #endregion

        #region UDS Service: ReadDataByIdentifier
        // ISO-15765-3:2004 §9.3.1 p.47 && ISO-14229-1:2006 §10.2 p.97

        /// <summary>
        /// Data Identifiers ISO-14229-1:2006 §C.1 p.259
        /// </summary>
        public enum TPUDSSvcParamDI : ushort
        {
            /// <summary>
            /// pack_and_cell_data.cell_v.min.val
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MIN_VAL_DID = 1,

            /// <summary>
            /// pack_and_cell_data.cell_v.min.id.cmu
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MIN_ID_CMU_DID = 2,

            /// <summary>
            /// pack_and_cell_data.cell_v.min.id.cell
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MIN_ID_CELL_DID = 3,

            /// <summary>
            /// pack_and_cell_data.cell_v.max.val
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MAX_VAL_DID = 4,

            /// <summary>
            /// pack_and_cell_data.cell_v.max.id.cmu
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MAX_ID_CMU_DID = 5,

            /// <summary>
            /// pack_and_cell_data.cell_v.max.id.cell
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MAX_ID_CELL_DID = 6,

            /// <summary>
            /// pack_and_cell_data.cell_v.avg
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_AVG_DID = 7,

            /// <summary>
            /// pack_and_cell_data.cell_v.num_avaliable
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_NUM_AVALIABLE_DID = 8,

            /// <summary>
            /// pack_and_cell_data.pack_v.ext_load
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_EXT_LOAD_DID = 9,

            /// <summary>
            /// pack_and_cell_data.pack_v.ext_chg
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_EXT_CHG_DID = 10,

            /// <summary>
            /// pack_and_cell_data.pack_v.sum_of_cells
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_SUM_OF_CELLS_DID = 11,

            /// <summary>
            /// pack_and_cell_data.pack_i.hall_v_lo
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_HALL_V_LO_DID = 12,

            /// <summary>
            /// pack_and_cell_data.pack_i.hall_v_hi
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_HALL_V_HI_DID = 13,

            /// <summary>
            /// pack_and_cell_data.pack_i.hall
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_HALL_DID = 14,

            /// <summary>
            /// pack_and_cell_data.pack_i.shunt
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_SHUNT_DID = 15,

            /// <summary>
            /// pack_and_cell_data.pack_i.master
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_MASTER_DID = 16,

            /// <summary>
            /// pack_and_cell_data.pack_q.remaining_hi_res
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_REMAINING_HI_RES_DID = 17,

            /// <summary>
            /// pack_and_cell_data.pack_q.soc_internal
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_SOC_INTERNAL_DID = 18,

            /// <summary>
            /// pack_and_cell_data.pack_q.soc_trimmed
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_SOC_TRIMMED_DID = 19,

            /// <summary>
            /// pack_and_cell_data.pack_q.remaining_nominal
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_REMAINING_NOMINAL_DID = 20,

            /// <summary>
            /// pack_and_cell_data.pack_q.design
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_DESIGN_DID = 21,

            /// <summary>
            /// pack_and_cell_data.pack_q.full
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_FULL_DID = 22,

            /// <summary>
            /// state_data.io_state.output
            /// </summary>
            PUDS_SVC_PARAM_DI_IO_STATE_OUTPUT_DID = 23,

            /// <summary>
            /// state_data.io_state.input
            /// </summary>
            PUDS_SVC_PARAM_DI_IO_STATE_INPUT_DID = 24,

            /// <summary>
            /// state_data.charger.output_enabled
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_OUTPUT_ENABLED_DID = 25,

            /// <summary>
            /// state_data.charger.output_current
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_OUTPUT_CURRENT_DID = 26,

            /// <summary>
            /// state_data.charger.output_voltage
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_OUTPUT_VOLTAGE_DID = 27,

            /// <summary>
            /// state_data.charger.can_active
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_CAN_ACTIVE_DID = 28,

            /// <summary>
            /// state_data.charger.pwm_active
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_PWM_ACTIVE_DID = 29,

            /// <summary>
            /// state_data.charger.pwm_active_duty
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_PWM_ACTIVE_DUTY_DID = 30,

            /// <summary>
            /// state_data.dyn_lim.i2t_remain
            /// </summary>
            PUDS_SVC_PARAM_DI_DYN_LIM_I2T_REMAIN_DID = 31,

            /// <summary>
            /// state_data.dyn_lim.i_in
            /// </summary>
            PUDS_SVC_PARAM_DI_DYN_LIM_I_IN_DID = 32,

            /// <summary>
            /// state_data.dyn_lim.i_out
            /// </summary>
            PUDS_SVC_PARAM_DI_DYN_LIM_I_OUT_DID = 33,

            /// <summary>
            /// state_data.status
            /// </summary>
            PUDS_SVC_PARAM_DI_STATUS_DID = 34,

            /// <summary>
            /// state_data.contactors.enabled
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ENABLED_DID = 35,

            /// <summary>
            /// state_data.contactors.activate_charge
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ACTIVATE_CHARGE_DID = 36,

            /// <summary>
            /// state_data.contactors.activate_load
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ACTIVATE_LOAD_DID = 37,

            /// <summary>
            /// state_data.contactors.activate_combined
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ACTIVATE_COMBINED_DID = 38,

            /// <summary>
            /// state_data.contactors.charger_activated
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_CHARGER_ACTIVATED_DID = 39,

            /// <summary>
            /// state_data.contactors.load_activated
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_LOAD_ACTIVATED_DID = 40,

            /// <summary>
            /// state_data.contactors.combined_activated
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_COMBINED_ACTIVATED_DID = 41,

            /// <summary>
            /// state_data.contactors.activation_allowed
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ACTIVATION_ALLOWED_DID = 42,

            /// <summary>
            /// state_data.contactors.emergency_off
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_EMERGENCY_OFF_DID = 43,

            /// <summary>
            /// state_data.contactors.contactor_retries
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_CONTACTOR_RETRIES_DID = 44,

            /// <summary>
            /// state_data.balancing.allowed
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_ALLOWED_DID = 45,

            /// <summary>
            /// state_data.balancing.limit
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_LIMIT_DID = 46,

            /// <summary>
            /// state_data.flags.fully_charged
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_FULLY_CHARGED_DID = 47,

            /// <summary>
            /// state_data.flags.fully_charged_latched
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_FULLY_CHARGED_LATCHED_DID = 48,

            /// <summary>
            /// state_data.flags.load_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_LOAD_ACTIVE_DID = 49,

            /// <summary>
            /// state_data.flags.charger_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_CHARGER_ACTIVE_DID = 50,

            /// <summary>
            /// state_data.flags.precharge_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_PRECHARGE_ACTIVE_DID = 51,

            /// <summary>
            /// state_data.flags.balancing_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_BALANCING_ACTIVE_DID = 52,

            /// <summary>
            /// state_data.flags.charge_reg_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_CHARGE_REG_ACTIVE_DID = 53,

            /// <summary>
            /// psu_status.b_wu_ign
            /// </summary>
            PUDS_SVC_PARAM_DI_B_WU_IGN_DID = 54,

            /// <summary>
            /// psu_status.b_wu_can
            /// </summary>
            PUDS_SVC_PARAM_DI_B_WU_CAN_DID = 55,

            /// <summary>
            /// psu_status.vcp
            /// </summary>
            PUDS_SVC_PARAM_DI_VCP_DID = 56,

            /// <summary>
            /// psu_status.vbat
            /// </summary>
            PUDS_SVC_PARAM_DI_VBAT_DID = 57,

            /// <summary>
            /// psu_status.main
            /// </summary>
            PUDS_SVC_PARAM_DI_MAIN_DID = 58,

            /// <summary>
            /// psu_status.vmon
            /// </summary>
            PUDS_SVC_PARAM_DI_VMON_DID = 59,

            /// <summary>
            /// error_status.critical_severity_count
            /// </summary>
            PUDS_SVC_PARAM_DI_CRITICAL_SEVERITY_COUNT_DID = 60,

            /// <summary>
            /// error_status.normal_severity_count
            /// </summary>
            PUDS_SVC_PARAM_DI_NORMAL_SEVERITY_COUNT_DID = 61,

            /// <summary>
            /// error_status.low_severity_count
            /// </summary>
            PUDS_SVC_PARAM_DI_LOW_SEVERITY_COUNT_DID = 62,

            /// <summary>
            /// error_status.active_count
            /// </summary>
            PUDS_SVC_PARAM_DI_ACTIVE_COUNT_DID = 63,

            /// <summary>
            /// version_entry.project
            /// </summary>
            PUDS_SVC_PARAM_DI_PROJECT_DID = 64,

            /// <summary>
            /// version_entry.hw_pcb
            /// </summary>
            PUDS_SVC_PARAM_DI_HW_PCB_DID = 65,

            /// <summary>
            /// version_entry.hw_bom
            /// </summary>
            PUDS_SVC_PARAM_DI_HW_BOM_DID = 66,

            /// <summary>
            /// version_entry.fw_major
            /// </summary>
            PUDS_SVC_PARAM_DI_FW_MAJOR_DID = 67,

            /// <summary>
            /// version_entry.fw_minor
            /// </summary>
            PUDS_SVC_PARAM_DI_FW_MINOR_DID = 68,

            /// <summary>
            /// version_entry.type
            /// </summary>
            PUDS_SVC_PARAM_DI_TYPE_DID = 69,

            /// <summary>
            /// version_entry.build_date
            /// </summary>
            PUDS_SVC_PARAM_DI_BUILD_DATE_DID = 70,

            /// <summary>
            /// version_entry.build_time
            /// </summary>
            PUDS_SVC_PARAM_DI_BUILD_TIME_DID = 71,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb0
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB0_DID = 72,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb1
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB1_DID = 73,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb2
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB2_DID = 74,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb3
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB3_DID = 75,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb4
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB4_DID = 76,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb5
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB5_DID = 77,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb6
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB6_DID = 78,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb7
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB7_DID = 79,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb8
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB8_DID = 80,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb9
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB9_DID = 81,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb10
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB10_DID = 82,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb11
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB11_DID = 83,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb12
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB12_DID = 84,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb13
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB13_DID = 85,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb14
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB14_DID = 86,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb15
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB15_DID = 87,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb16
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB16_DID = 88,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb17
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB17_DID = 89,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb18
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB18_DID = 90,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb19
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB19_DID = 91,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb20
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB20_DID = 92,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb21
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB21_DID = 93,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb22
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB22_DID = 94,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb23
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB23_DID = 95,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb24
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB24_DID = 96,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb25
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB25_DID = 97,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb26
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB26_DID = 98,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb27
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB27_DID = 99,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb28
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB28_DID = 100,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb29
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB29_DID = 101,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb30
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB30_DID = 102,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb31
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB31_DID = 103,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb32
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB32_DID = 104,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb33
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB33_DID = 105,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb34
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB34_DID = 106,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb35
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB35_DID = 107,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb36
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB36_DID = 108,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb37
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB37_DID = 109,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb38
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB38_DID = 110,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb39
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB39_DID = 111,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb40
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB40_DID = 112,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb41
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB41_DID = 113,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb42
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB42_DID = 114,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb43
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB43_DID = 115,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb44
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB44_DID = 116,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb45
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB45_DID = 117,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb46
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB46_DID = 118,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb47
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB47_DID = 119,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb48
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB48_DID = 120,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb49
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB49_DID = 121,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb50
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB50_DID = 122,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb51
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB51_DID = 123,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb52
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB52_DID = 124,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb53
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB53_DID = 125,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb54
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB54_DID = 126,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb55
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB55_DID = 127,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb56
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB56_DID = 128,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb57
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB57_DID = 129,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb58
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB58_DID = 130,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb59
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB59_DID = 131,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb60
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB60_DID = 132,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb61
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB61_DID = 133,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb62
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB62_DID = 134,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb63
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB63_DID = 135,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb_gpio
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB_GPIO_DID = 136,

            /// <summary>
            /// pack_and_cell_data.pcb_t.pcb_psu
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T_PCB_PSU_DID = 137,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux0
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX0_DID = 138,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux1
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX1_DID = 139,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux2
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX2_DID = 140,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux3
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX3_DID = 141,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux4
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX4_DID = 142,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux5
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX5_DID = 143,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux6
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX6_DID = 144,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux7
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX7_DID = 145,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux8
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX8_DID = 146,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux9
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX9_DID = 147,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux10
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX10_DID = 148,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux11
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX11_DID = 149,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux12
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX12_DID = 150,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux13
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX13_DID = 151,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux14
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX14_DID = 152,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux15
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX15_DID = 153,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux16
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX16_DID = 154,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux17
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX17_DID = 155,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux18
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX18_DID = 156,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux19
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX19_DID = 157,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux20
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX20_DID = 158,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux21
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX21_DID = 159,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux22
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX22_DID = 160,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux23
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX23_DID = 161,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux24
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX24_DID = 162,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux25
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX25_DID = 163,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux26
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX26_DID = 164,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux27
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX27_DID = 165,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux28
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX28_DID = 166,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux29
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX29_DID = 167,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux30
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX30_DID = 168,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux31
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX31_DID = 169,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux32
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX32_DID = 170,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux33
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX33_DID = 171,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux34
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX34_DID = 172,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux35
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX35_DID = 173,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux36
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX36_DID = 174,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux37
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX37_DID = 175,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux38
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX38_DID = 176,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux39
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX39_DID = 177,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux40
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX40_DID = 178,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux41
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX41_DID = 179,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux42
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX42_DID = 180,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux43
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX43_DID = 181,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux44
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX44_DID = 182,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux45
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX45_DID = 183,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux46
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX46_DID = 184,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux47
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX47_DID = 185,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux48
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX48_DID = 186,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux49
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX49_DID = 187,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux50
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX50_DID = 188,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux51
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX51_DID = 189,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux52
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX52_DID = 190,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux53
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX53_DID = 191,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux54
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX54_DID = 192,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux55
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX55_DID = 193,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux56
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX56_DID = 194,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux57
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX57_DID = 195,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux58
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX58_DID = 196,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux59
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX59_DID = 197,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux60
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX60_DID = 198,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux61
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX61_DID = 199,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux62
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX62_DID = 200,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux63
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX63_DID = 201,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux64
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX64_DID = 202,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux65
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX65_DID = 203,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux66
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX66_DID = 204,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux67
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX67_DID = 205,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux68
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX68_DID = 206,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux69
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX69_DID = 207,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux70
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX70_DID = 208,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux71
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX71_DID = 209,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux72
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX72_DID = 210,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux73
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX73_DID = 211,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux74
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX74_DID = 212,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux75
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX75_DID = 213,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux76
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX76_DID = 214,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux77
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX77_DID = 215,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux78
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX78_DID = 216,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux79
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX79_DID = 217,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux80
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX80_DID = 218,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux81
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX81_DID = 219,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux82
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX82_DID = 220,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux83
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX83_DID = 221,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux84
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX84_DID = 222,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux85
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX85_DID = 223,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux86
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX86_DID = 224,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux87
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX87_DID = 225,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux88
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX88_DID = 226,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux89
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX89_DID = 227,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux90
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX90_DID = 228,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux91
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX91_DID = 229,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux92
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX92_DID = 230,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux93
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX93_DID = 231,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux94
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX94_DID = 232,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux95
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX95_DID = 233,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux96
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX96_DID = 234,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux97
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX97_DID = 235,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux98
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX98_DID = 236,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux99
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX99_DID = 237,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux100
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX100_DID = 238,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux101
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX101_DID = 239,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux102
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX102_DID = 240,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux103
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX103_DID = 241,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux104
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX104_DID = 242,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux105
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX105_DID = 243,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux106
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX106_DID = 244,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux107
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX107_DID = 245,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux108
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX108_DID = 246,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux109
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX109_DID = 247,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux110
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX110_DID = 248,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux111
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX111_DID = 249,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux112
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX112_DID = 250,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux113
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX113_DID = 251,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux114
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX114_DID = 252,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux115
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX115_DID = 253,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux116
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX116_DID = 254,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux117
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX117_DID = 255,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux118
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX118_DID = 256,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux119
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX119_DID = 257,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux120
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX120_DID = 258,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux121
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX121_DID = 259,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux122
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX122_DID = 260,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux123
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX123_DID = 261,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux124
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX124_DID = 262,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux125
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX125_DID = 263,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux126
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX126_DID = 264,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux127
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX127_DID = 265,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux128
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX128_DID = 266,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux129
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX129_DID = 267,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux130
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX130_DID = 268,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux131
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX131_DID = 269,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux132
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX132_DID = 270,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux133
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX133_DID = 271,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux134
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX134_DID = 272,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux135
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX135_DID = 273,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux136
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX136_DID = 274,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux137
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX137_DID = 275,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux138
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX138_DID = 276,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux139
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX139_DID = 277,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux140
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX140_DID = 278,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux141
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX141_DID = 279,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux142
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX142_DID = 280,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux143
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX143_DID = 281,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux144
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX144_DID = 282,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux145
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX145_DID = 283,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux146
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX146_DID = 284,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux147
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX147_DID = 285,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux148
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX148_DID = 286,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux149
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX149_DID = 287,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux150
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX150_DID = 288,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux151
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX151_DID = 289,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux152
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX152_DID = 290,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux153
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX153_DID = 291,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux154
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX154_DID = 292,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux155
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX155_DID = 293,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux156
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX156_DID = 294,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux157
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX157_DID = 295,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux158
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX158_DID = 296,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux159
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX159_DID = 297,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux160
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX160_DID = 298,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux161
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX161_DID = 299,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux162
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX162_DID = 300,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux163
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX163_DID = 301,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux164
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX164_DID = 302,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux165
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX165_DID = 303,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux166
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX166_DID = 304,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux167
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX167_DID = 305,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux168
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX168_DID = 306,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux169
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX169_DID = 307,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux170
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX170_DID = 308,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux171
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX171_DID = 309,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux172
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX172_DID = 310,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux173
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX173_DID = 311,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux174
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX174_DID = 312,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux175
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX175_DID = 313,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux176
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX176_DID = 314,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux177
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX177_DID = 315,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux178
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX178_DID = 316,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux179
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX179_DID = 317,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux180
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX180_DID = 318,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux181
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX181_DID = 319,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux182
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX182_DID = 320,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux183
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX183_DID = 321,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux184
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX184_DID = 322,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux185
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX185_DID = 323,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux186
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX186_DID = 324,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux187
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX187_DID = 325,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux188
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX188_DID = 326,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux189
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX189_DID = 327,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux190
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX190_DID = 328,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux191
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX191_DID = 329,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux192
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX192_DID = 330,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux193
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX193_DID = 331,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux194
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX194_DID = 332,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux195
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX195_DID = 333,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux196
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX196_DID = 334,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux197
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX197_DID = 335,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux198
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX198_DID = 336,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux199
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX199_DID = 337,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux200
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX200_DID = 338,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux201
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX201_DID = 339,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux202
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX202_DID = 340,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux203
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX203_DID = 341,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux204
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX204_DID = 342,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux205
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX205_DID = 343,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux206
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX206_DID = 344,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux207
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX207_DID = 345,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux208
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX208_DID = 346,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux209
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX209_DID = 347,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux210
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX210_DID = 348,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux211
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX211_DID = 349,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux212
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX212_DID = 350,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux213
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX213_DID = 351,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux214
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX214_DID = 352,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux215
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX215_DID = 353,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux216
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX216_DID = 354,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux217
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX217_DID = 355,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux218
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX218_DID = 356,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux219
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX219_DID = 357,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux220
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX220_DID = 358,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux221
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX221_DID = 359,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux222
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX222_DID = 360,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux223
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX223_DID = 361,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux224
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX224_DID = 362,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux225
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX225_DID = 363,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux226
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX226_DID = 364,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux227
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX227_DID = 365,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux228
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX228_DID = 366,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux229
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX229_DID = 367,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux230
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX230_DID = 368,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux231
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX231_DID = 369,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux232
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX232_DID = 370,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux233
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX233_DID = 371,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux234
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX234_DID = 372,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux235
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX235_DID = 373,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux236
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX236_DID = 374,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux237
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX237_DID = 375,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux238
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX238_DID = 376,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux239
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX239_DID = 377,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux240
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX240_DID = 378,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux241
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX241_DID = 379,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux242
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX242_DID = 380,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux243
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX243_DID = 381,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux244
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX244_DID = 382,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux245
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX245_DID = 383,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux246
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX246_DID = 384,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux247
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX247_DID = 385,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux248
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX248_DID = 386,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux249
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX249_DID = 387,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux250
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX250_DID = 388,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux251
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX251_DID = 389,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux252
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX252_DID = 390,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux253
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX253_DID = 391,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux254
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX254_DID = 392,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux255
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX255_DID = 393,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux256
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX256_DID = 394,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux257
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX257_DID = 395,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux258
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX258_DID = 396,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux259
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX259_DID = 397,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux260
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX260_DID = 398,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux261
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX261_DID = 399,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux262
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX262_DID = 400,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux263
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX263_DID = 401,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux264
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX264_DID = 402,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux265
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX265_DID = 403,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux266
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX266_DID = 404,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux267
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX267_DID = 405,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux268
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX268_DID = 406,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux269
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX269_DID = 407,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux270
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX270_DID = 408,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux271
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX271_DID = 409,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux272
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX272_DID = 410,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux273
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX273_DID = 411,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux274
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX274_DID = 412,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux275
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX275_DID = 413,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux276
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX276_DID = 414,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux277
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX277_DID = 415,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux278
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX278_DID = 416,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux279
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX279_DID = 417,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux280
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX280_DID = 418,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux281
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX281_DID = 419,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux282
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX282_DID = 420,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux283
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX283_DID = 421,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux284
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX284_DID = 422,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux285
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX285_DID = 423,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux286
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX286_DID = 424,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux287
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX287_DID = 425,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux288
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX288_DID = 426,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux289
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX289_DID = 427,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux290
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX290_DID = 428,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux291
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX291_DID = 429,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux292
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX292_DID = 430,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux293
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX293_DID = 431,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux294
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX294_DID = 432,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux295
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX295_DID = 433,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux296
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX296_DID = 434,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux297
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX297_DID = 435,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux298
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX298_DID = 436,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux299
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX299_DID = 437,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux300
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX300_DID = 438,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux301
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX301_DID = 439,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux302
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX302_DID = 440,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux303
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX303_DID = 441,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux304
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX304_DID = 442,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux305
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX305_DID = 443,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux306
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX306_DID = 444,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux307
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX307_DID = 445,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux308
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX308_DID = 446,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux309
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX309_DID = 447,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux310
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX310_DID = 448,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux311
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX311_DID = 449,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux312
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX312_DID = 450,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux313
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX313_DID = 451,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux314
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX314_DID = 452,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux315
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX315_DID = 453,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux316
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX316_DID = 454,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux317
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX317_DID = 455,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux318
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX318_DID = 456,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux319
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX319_DID = 457,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux320
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX320_DID = 458,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux321
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX321_DID = 459,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux322
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX322_DID = 460,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux323
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX323_DID = 461,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux324
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX324_DID = 462,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux325
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX325_DID = 463,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux326
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX326_DID = 464,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux327
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX327_DID = 465,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux328
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX328_DID = 466,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux329
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX329_DID = 467,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux330
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX330_DID = 468,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux331
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX331_DID = 469,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux332
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX332_DID = 470,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux333
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX333_DID = 471,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux334
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX334_DID = 472,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux335
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX335_DID = 473,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux336
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX336_DID = 474,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux337
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX337_DID = 475,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux338
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX338_DID = 476,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux339
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX339_DID = 477,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux340
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX340_DID = 478,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux341
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX341_DID = 479,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux342
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX342_DID = 480,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux343
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX343_DID = 481,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux344
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX344_DID = 482,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux345
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX345_DID = 483,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux346
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX346_DID = 484,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux347
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX347_DID = 485,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux348
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX348_DID = 486,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux349
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX349_DID = 487,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux350
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX350_DID = 488,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux351
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX351_DID = 489,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux352
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX352_DID = 490,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux353
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX353_DID = 491,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux354
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX354_DID = 492,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux355
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX355_DID = 493,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux356
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX356_DID = 494,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux357
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX357_DID = 495,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux358
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX358_DID = 496,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux359
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX359_DID = 497,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux360
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX360_DID = 498,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux361
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX361_DID = 499,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux362
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX362_DID = 500,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux363
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX363_DID = 501,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux364
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX364_DID = 502,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux365
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX365_DID = 503,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux366
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX366_DID = 504,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux367
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX367_DID = 505,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux368
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX368_DID = 506,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux369
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX369_DID = 507,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux370
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX370_DID = 508,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux371
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX371_DID = 509,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux372
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX372_DID = 510,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux373
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX373_DID = 511,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux374
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX374_DID = 512,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux375
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX375_DID = 513,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux376
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX376_DID = 514,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux377
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX377_DID = 515,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux378
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX378_DID = 516,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux379
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX379_DID = 517,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux380
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX380_DID = 518,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux381
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX381_DID = 519,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux382
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX382_DID = 520,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux383
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX383_DID = 521,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux384
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX384_DID = 522,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux385
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX385_DID = 523,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux386
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX386_DID = 524,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux387
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX387_DID = 525,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux388
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX388_DID = 526,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux389
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX389_DID = 527,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux390
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX390_DID = 528,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux391
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX391_DID = 529,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux392
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX392_DID = 530,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux393
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX393_DID = 531,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux394
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX394_DID = 532,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux395
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX395_DID = 533,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux396
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX396_DID = 534,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux397
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX397_DID = 535,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux398
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX398_DID = 536,

            /// <summary>
            /// pack_and_cell_data.pack_t.aux399
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_T_AUX399_DID = 537,

            /// <summary>
            /// pack_and_cell_data.cmus.online_count
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_ONLINE_COUNT_DID = 538,

            /// <summary>
            /// pack_and_cell_data.cmus.comm_err
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_COMM_ERR_DID = 539,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V0_DID = 540,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V1_DID = 541,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V2_DID = 542,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V3_DID = 543,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V4_DID = 544,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V5_DID = 545,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V6_DID = 546,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V7_DID = 547,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V8_DID = 548,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V9_DID = 549,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V10_DID = 550,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu0.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU0_CELL_V11_DID = 551,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V0_DID = 552,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V1_DID = 553,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V2_DID = 554,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V3_DID = 555,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V4_DID = 556,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V5_DID = 557,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V6_DID = 558,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V7_DID = 559,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V8_DID = 560,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V9_DID = 561,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V10_DID = 562,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V11_DID = 563,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V0_DID = 564,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V1_DID = 565,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V2_DID = 566,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V3_DID = 567,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V4_DID = 568,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V5_DID = 569,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V6_DID = 570,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V7_DID = 571,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V8_DID = 572,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V9_DID = 573,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V10_DID = 574,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V11_DID = 575,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V0_DID = 576,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V1_DID = 577,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V2_DID = 578,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V3_DID = 579,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V4_DID = 580,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V5_DID = 581,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V6_DID = 582,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V7_DID = 583,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V8_DID = 584,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V9_DID = 585,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V10_DID = 586,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V11_DID = 587,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V0_DID = 588,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V1_DID = 589,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V2_DID = 590,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V3_DID = 591,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V4_DID = 592,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V5_DID = 593,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V6_DID = 594,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V7_DID = 595,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V8_DID = 596,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V9_DID = 597,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V10_DID = 598,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V11_DID = 599,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V0_DID = 600,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V1_DID = 601,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V2_DID = 602,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V3_DID = 603,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V4_DID = 604,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V5_DID = 605,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V6_DID = 606,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V7_DID = 607,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V8_DID = 608,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V9_DID = 609,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V10_DID = 610,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V11_DID = 611,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V0_DID = 612,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V1_DID = 613,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V2_DID = 614,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V3_DID = 615,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V4_DID = 616,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V5_DID = 617,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V6_DID = 618,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V7_DID = 619,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V8_DID = 620,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V9_DID = 621,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V10_DID = 622,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V11_DID = 623,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V0_DID = 624,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V1_DID = 625,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V2_DID = 626,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V3_DID = 627,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V4_DID = 628,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V5_DID = 629,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V6_DID = 630,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V7_DID = 631,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V8_DID = 632,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V9_DID = 633,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V10_DID = 634,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V11_DID = 635,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V0_DID = 636,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V1_DID = 637,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V2_DID = 638,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V3_DID = 639,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V4_DID = 640,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V5_DID = 641,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V6_DID = 642,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V7_DID = 643,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V8_DID = 644,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V9_DID = 645,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V10_DID = 646,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V11_DID = 647,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V0_DID = 648,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V1_DID = 649,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V2_DID = 650,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V3_DID = 651,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V4_DID = 652,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V5_DID = 653,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V6_DID = 654,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V7_DID = 655,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V8_DID = 656,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V9_DID = 657,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V10_DID = 658,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V11_DID = 659,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V0_DID = 660,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V1_DID = 661,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V2_DID = 662,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V3_DID = 663,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V4_DID = 664,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V5_DID = 665,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V6_DID = 666,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V7_DID = 667,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V8_DID = 668,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V9_DID = 669,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V10_DID = 670,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V11_DID = 671,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V0_DID = 672,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V1_DID = 673,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V2_DID = 674,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V3_DID = 675,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V4_DID = 676,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V5_DID = 677,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V6_DID = 678,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V7_DID = 679,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V8_DID = 680,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V9_DID = 681,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V10_DID = 682,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V11_DID = 683,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V0_DID = 684,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V1_DID = 685,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V2_DID = 686,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V3_DID = 687,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V4_DID = 688,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V5_DID = 689,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V6_DID = 690,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V7_DID = 691,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V8_DID = 692,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V9_DID = 693,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V10_DID = 694,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V11_DID = 695,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V0_DID = 696,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V1_DID = 697,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V2_DID = 698,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V3_DID = 699,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V4_DID = 700,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V5_DID = 701,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V6_DID = 702,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V7_DID = 703,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V8_DID = 704,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V9_DID = 705,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V10_DID = 706,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V11_DID = 707,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V0_DID = 708,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V1_DID = 709,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V2_DID = 710,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V3_DID = 711,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V4_DID = 712,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V5_DID = 713,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V6_DID = 714,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V7_DID = 715,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V8_DID = 716,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V9_DID = 717,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V10_DID = 718,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V11_DID = 719,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V0_DID = 720,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V1_DID = 721,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V2_DID = 722,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V3_DID = 723,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V4_DID = 724,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V5_DID = 725,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V6_DID = 726,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V7_DID = 727,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V8_DID = 728,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V9_DID = 729,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V10_DID = 730,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V11_DID = 731,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V0_DID = 732,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V1_DID = 733,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V2_DID = 734,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V3_DID = 735,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V4_DID = 736,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V5_DID = 737,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V6_DID = 738,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V7_DID = 739,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V8_DID = 740,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V9_DID = 741,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V10_DID = 742,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V11_DID = 743,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V0_DID = 744,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V1_DID = 745,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V2_DID = 746,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V3_DID = 747,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V4_DID = 748,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V5_DID = 749,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V6_DID = 750,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V7_DID = 751,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V8_DID = 752,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V9_DID = 753,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V10_DID = 754,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V11_DID = 755,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V0_DID = 756,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V1_DID = 757,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V2_DID = 758,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V3_DID = 759,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V4_DID = 760,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V5_DID = 761,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V6_DID = 762,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V7_DID = 763,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V8_DID = 764,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V9_DID = 765,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V10_DID = 766,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V11_DID = 767,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V0_DID = 768,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V1_DID = 769,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V2_DID = 770,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V3_DID = 771,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V4_DID = 772,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V5_DID = 773,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V6_DID = 774,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V7_DID = 775,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V8_DID = 776,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V9_DID = 777,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V10_DID = 778,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V11_DID = 779,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V0_DID = 780,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V1_DID = 781,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V2_DID = 782,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V3_DID = 783,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V4_DID = 784,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V5_DID = 785,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V6_DID = 786,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V7_DID = 787,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V8_DID = 788,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V9_DID = 789,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V10_DID = 790,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V11_DID = 791,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V0_DID = 792,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V1_DID = 793,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V2_DID = 794,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V3_DID = 795,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V4_DID = 796,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V5_DID = 797,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V6_DID = 798,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V7_DID = 799,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V8_DID = 800,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V9_DID = 801,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V10_DID = 802,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V11_DID = 803,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V0_DID = 804,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V1_DID = 805,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V2_DID = 806,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V3_DID = 807,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V4_DID = 808,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V5_DID = 809,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V6_DID = 810,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V7_DID = 811,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V8_DID = 812,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V9_DID = 813,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V10_DID = 814,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V11_DID = 815,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V0_DID = 816,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V1_DID = 817,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V2_DID = 818,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V3_DID = 819,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V4_DID = 820,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V5_DID = 821,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V6_DID = 822,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V7_DID = 823,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V8_DID = 824,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V9_DID = 825,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V10_DID = 826,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V11_DID = 827,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V0_DID = 828,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V1_DID = 829,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V2_DID = 830,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V3_DID = 831,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V4_DID = 832,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V5_DID = 833,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V6_DID = 834,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V7_DID = 835,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V8_DID = 836,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V9_DID = 837,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V10_DID = 838,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V11_DID = 839,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V0_DID = 840,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V1_DID = 841,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V2_DID = 842,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V3_DID = 843,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V4_DID = 844,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V5_DID = 845,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V6_DID = 846,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V7_DID = 847,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V8_DID = 848,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V9_DID = 849,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V10_DID = 850,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V11_DID = 851,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V0_DID = 852,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V1_DID = 853,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V2_DID = 854,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V3_DID = 855,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V4_DID = 856,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V5_DID = 857,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V6_DID = 858,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V7_DID = 859,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V8_DID = 860,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V9_DID = 861,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V10_DID = 862,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V11_DID = 863,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V0_DID = 864,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V1_DID = 865,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V2_DID = 866,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V3_DID = 867,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V4_DID = 868,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V5_DID = 869,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V6_DID = 870,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V7_DID = 871,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V8_DID = 872,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V9_DID = 873,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V10_DID = 874,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V11_DID = 875,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V0_DID = 876,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V1_DID = 877,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V2_DID = 878,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V3_DID = 879,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V4_DID = 880,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V5_DID = 881,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V6_DID = 882,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V7_DID = 883,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V8_DID = 884,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V9_DID = 885,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V10_DID = 886,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V11_DID = 887,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V0_DID = 888,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V1_DID = 889,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V2_DID = 890,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V3_DID = 891,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V4_DID = 892,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V5_DID = 893,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V6_DID = 894,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V7_DID = 895,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V8_DID = 896,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V9_DID = 897,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V10_DID = 898,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V11_DID = 899,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V0_DID = 900,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V1_DID = 901,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V2_DID = 902,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V3_DID = 903,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V4_DID = 904,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V5_DID = 905,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V6_DID = 906,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V7_DID = 907,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V8_DID = 908,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V9_DID = 909,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V10_DID = 910,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V11_DID = 911,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v0
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V0_DID = 912,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V1_DID = 913,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V2_DID = 914,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V3_DID = 915,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V4_DID = 916,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V5_DID = 917,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V6_DID = 918,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V7_DID = 919,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V8_DID = 920,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V9_DID = 921,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V10_DID = 922,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V11_DID = 923,

            // Commented data id's existing from 924 - 955 and added same dataids from MsNp as MsNp have same data ids with meaningful text. 

            ///// <summary>
            ///// rtos_status.freertos_free_heap_size
            ///// </summary>
            //PUDS_SVC_PARAM_DI_FREERTOS_FREE_HEAP_SIZE_DID = 924,

            ///// <summary>
            ///// rtos_status.freertos_min_ever_free_heap
            ///// </summary>
            //PUDS_SVC_PARAM_DI_FREERTOS_MIN_EVER_FREE_HEAP_DID = 925,

            ///// <summary>
            ///// rtos_status.no_of_tasks
            ///// </summary>
            //PUDS_SVC_PARAM_DI_NO_OF_TASKS_DID = 926,

            ///// <summary>
            ///// rtos_status.task_info0.name
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO0_NAME_DID = 927,

            ///// <summary>
            ///// rtos_status.task_info0.handle
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO0_HANDLE_DID = 928,

            ///// <summary>
            ///// rtos_status.task_info0.run_time_total
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO0_RUN_TIME_TOTAL_DID = 929,

            ///// <summary>
            ///// rtos_status.task_info0.run_time_pct
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO0_RUN_TIME_PCT_DID = 930,

            ///// <summary>
            ///// rtos_status.task_info0.minimum_stack
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO0_MINIMUM_STACK_DID = 931,

            ///// <summary>
            ///// rtos_status.task_info1.name
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO1_NAME_DID = 932,

            ///// <summary>
            ///// rtos_status.task_info1.handle
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO1_HANDLE_DID = 933,

            ///// <summary>
            ///// rtos_status.task_info1.run_time_total
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO1_RUN_TIME_TOTAL_DID = 934,

            ///// <summary>
            ///// rtos_status.task_info1.run_time_pct
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO1_RUN_TIME_PCT_DID = 935,

            ///// <summary>
            ///// rtos_status.task_info1.minimum_stack
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO1_MINIMUM_STACK_DID = 936,

            ///// <summary>
            ///// rtos_status.task_info2.name
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO2_NAME_DID = 937,

            ///// <summary>
            ///// rtos_status.task_info2.handle
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO2_HANDLE_DID = 938,

            ///// <summary>
            ///// rtos_status.task_info2.run_time_total
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO2_RUN_TIME_TOTAL_DID = 939,

            ///// <summary>
            ///// rtos_status.task_info2.run_time_pct
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO2_RUN_TIME_PCT_DID = 940,

            ///// <summary>
            ///// rtos_status.task_info2.minimum_stack
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO2_MINIMUM_STACK_DID = 941,

            ///// <summary>
            ///// rtos_status.task_info3.name
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO3_NAME_DID = 942,

            ///// <summary>
            ///// rtos_status.task_info3.handle
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO3_HANDLE_DID = 943,

            ///// <summary>
            ///// rtos_status.task_info3.run_time_total
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO3_RUN_TIME_TOTAL_DID = 944,

            ///// <summary>
            ///// rtos_status.task_info3.run_time_pct
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO3_RUN_TIME_PCT_DID = 945,

            ///// <summary>
            ///// rtos_status.task_info3.minimum_stack
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO3_MINIMUM_STACK_DID = 946,

            ///// <summary>
            ///// rtos_status.task_info4.name
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO4_NAME_DID = 947,

            ///// <summary>
            ///// rtos_status.task_info4.handle
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO4_HANDLE_DID = 948,

            ///// <summary>
            ///// rtos_status.task_info4.run_time_total
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO4_RUN_TIME_TOTAL_DID = 949,

            ///// <summary>
            ///// rtos_status.task_info4.run_time_pct
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO4_RUN_TIME_PCT_DID = 950,

            ///// <summary>
            ///// rtos_status.task_info4.minimum_stack
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO4_MINIMUM_STACK_DID = 951,

            ///// <summary>
            ///// rtos_status.task_info5.name
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO5_NAME_DID = 952,

            ///// <summary>
            ///// rtos_status.task_info5.handle
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO5_HANDLE_DID = 953,

            ///// <summary>
            ///// rtos_status.task_info5.run_time_total
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO5_RUN_TIME_TOTAL_DID = 954,

            ///// <summary>
            ///// rtos_status.task_info5.run_time_pct
            ///// </summary>
            //PUDS_SVC_PARAM_DI_TASK_INFO5_RUN_TIME_PCT_DID = 955,

            /// <summary>
            /// task_mon_data.min_free_heap
            /// </summary>
            PUDS_SVC_PARAM_DI_MIN_FREE_HEAP_DID = 924,

            /// <summary>
            /// task_mon_data.task_info0.id
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_ID_DID = 925,

            /// <summary>
            /// task_mon_data.task_info0.no_of_violations
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_NO_OF_VIOLATIONS_DID = 926,

            /// <summary>
            /// task_mon_data.task_info0.min_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_MIN_EXECUTION_TIME_MS_DID = 927,

            /// <summary>
            /// task_mon_data.task_info0.max_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_MAX_EXECUTION_TIME_MS_DID = 928,

            /// <summary>
            /// task_mon_data.task_info0.avg_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_AVG_EXECUTION_TIME_MS_DID = 929,

            /// <summary>
            /// task_mon_data.task_info0.min_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_MIN_TIME_BETWEEN_EXECUTIONS_MS_DID = 930,

            /// <summary>
            /// task_mon_data.task_info0.max_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_MAX_TIME_BETWEEN_EXECUTIONS_MS_DID = 931,

            /// <summary>
            /// task_mon_data.task_info0.avg_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_AVG_TIME_BETWEEN_EXECUTIONS_MS_DID = 932,

            /// <summary>
            /// task_mon_data.task_info0.min_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_MIN_EXECUTION_TIME_PCT_DID = 933,

            /// <summary>
            /// task_mon_data.task_info0.max_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_MAX_EXECUTION_TIME_PCT_DID = 934,

            /// <summary>
            /// task_mon_data.task_info0.avg_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_AVG_EXECUTION_TIME_PCT_DID = 935,

            /// <summary>
            /// task_mon_data.task_info0.min_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_MIN_TIME_BETWEEN_EXECUTIONS_PCT_DID = 936,

            /// <summary>
            /// task_mon_data.task_info0.max_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_MAX_TIME_BETWEEN_EXECUTIONS_PCT_DID = 937,

            /// <summary>
            /// task_mon_data.task_info0.avg_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO0_AVG_TIME_BETWEEN_EXECUTIONS_PCT_DID = 938,

            /// <summary>
            /// task_mon_data.task_info1.id
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_ID_DID = 939,

            /// <summary>
            /// task_mon_data.task_info1.no_of_violations
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_NO_OF_VIOLATIONS_DID = 940,

            /// <summary>
            /// task_mon_data.task_info1.min_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_MIN_EXECUTION_TIME_MS_DID = 941,

            /// <summary>
            /// task_mon_data.task_info1.max_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_MAX_EXECUTION_TIME_MS_DID = 942,

            /// <summary>
            /// task_mon_data.task_info1.avg_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_AVG_EXECUTION_TIME_MS_DID = 943,

            /// <summary>
            /// task_mon_data.task_info1.min_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_MIN_TIME_BETWEEN_EXECUTIONS_MS_DID = 944,

            /// <summary>
            /// task_mon_data.task_info1.max_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_MAX_TIME_BETWEEN_EXECUTIONS_MS_DID = 945,

            /// <summary>
            /// task_mon_data.task_info1.avg_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_AVG_TIME_BETWEEN_EXECUTIONS_MS_DID = 946,

            /// <summary>
            /// task_mon_data.task_info1.min_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_MIN_EXECUTION_TIME_PCT_DID = 947,

            /// <summary>
            /// task_mon_data.task_info1.max_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_MAX_EXECUTION_TIME_PCT_DID = 948,

            /// <summary>
            /// task_mon_data.task_info1.avg_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_AVG_EXECUTION_TIME_PCT_DID = 949,

            /// <summary>
            /// task_mon_data.task_info1.min_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_MIN_TIME_BETWEEN_EXECUTIONS_PCT_DID = 950,

            /// <summary>
            /// task_mon_data.task_info1.max_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_MAX_TIME_BETWEEN_EXECUTIONS_PCT_DID = 951,

            /// <summary>
            /// task_mon_data.task_info1.avg_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO1_AVG_TIME_BETWEEN_EXECUTIONS_PCT_DID = 952,

            /// <summary>
            /// task_mon_data.config_space_used_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIG_SPACE_USED_PCT_DID = 953,

            /// <summary>
            /// reserved954
            /// </summary>
            PUDS_SVC_PARAM_DI_RESERVED954_DID = 954,

            /// <summary>
            /// task_mon_data.time_between_boots_s
            /// </summary>
            PUDS_SVC_PARAM_DI_TIME_BETWEEN_BOOTS_S_DID = 955,


            /// <summary>
            /// rtos_status.epoch_time
            /// </summary>
            PUDS_SVC_PARAM_DI_EPOCH_TIME_S_DID = 956,

            /// <summary>
            /// error_status.system_uptime_in_sec
            /// </summary>
            PUDS_SVC_PARAM_DI_SYSTEM_UPTIME_IN_SEC_DID = 957,

            /// <summary>
            /// pack_and_cell_data.hall_ext_v.hall_v_lo
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_EXT_V_HALL_V_LO_DID = 958,

            /// <summary>
            /// pack_and_cell_data.hall_ext_v.hall_v_hi
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_EXT_V_HALL_V_HI_DID = 959,

            /// <summary>
            /// pack_and_cell_data.hall_ext_v.hall_v_supply
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_EXT_V_HALL_V_SUPPLY_DID = 960,

            /// <summary>
            /// pack_and_cell_data.hall_sys_v.hall_v_lo
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_SYS_V_HALL_V_LO_DID = 961,

            /// <summary>
            /// pack_and_cell_data.hall_sys_v.hall_v_hi
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_SYS_V_HALL_V_HI_DID = 962,

            /// <summary>
            /// pack_and_cell_data.hall_sys_v.hall_v_supply
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_SYS_V_HALL_V_SUPPLY_DID = 963,

            /// <summary>
            /// pack_and_cell_data.hall_v.hall_v_supply
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_V_HALL_V_SUPPLY_DID = 964,

            /// <summary>
            /// pack_and_cell_data.cell_t.min.val
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MIN_VAL_DID = 965,

            /// <summary>
            /// pack_and_cell_data.cell_t.min.id.cmu
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MIN_ID_CMU_DID = 966,

            /// <summary>
            /// pack_and_cell_data.cell_t.min.id.cell
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MIN_ID_CELL_DID = 967,

            /// <summary>
            /// pack_and_cell_data.cell_t.max.val
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MAX_VAL_DID = 968,

            /// <summary>
            /// pack_and_cell_data.cell_t.max.id.cmu
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MAX_ID_CMU_DID = 969,

            /// <summary>
            /// pack_and_cell_data.cell_t.max.id.cell
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MAX_ID_CELL_DID = 970,

            /// <summary>
            /// pack_and_cell_data.cell_t.avg
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_AVG_DID = 971,

            /// <summary>
            /// pack_and_cell_data.cell_t.num_available
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_NUM_AVAILABLE_DID = 972,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu0.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU0_CELL_BITMASK_DID = 973,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu1.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU1_CELL_BITMASK_DID = 974,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu2.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU2_CELL_BITMASK_DID = 975,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu3.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU3_CELL_BITMASK_DID = 976,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu4.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU4_CELL_BITMASK_DID = 977,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu5.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU5_CELL_BITMASK_DID = 978,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu6.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU6_CELL_BITMASK_DID = 979,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu7.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU7_CELL_BITMASK_DID = 980,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu8.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU8_CELL_BITMASK_DID = 981,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu9.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU9_CELL_BITMASK_DID = 982,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu10.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU10_CELL_BITMASK_DID = 983,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu11.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU11_CELL_BITMASK_DID = 984,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu12.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU12_CELL_BITMASK_DID = 985,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu13.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU13_CELL_BITMASK_DID = 986,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu14.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU14_CELL_BITMASK_DID = 987,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu15.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU15_CELL_BITMASK_DID = 988,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu16.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU16_CELL_BITMASK_DID = 989,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu17.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU17_CELL_BITMASK_DID = 990,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu18.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU18_CELL_BITMASK_DID = 991,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu19.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU19_CELL_BITMASK_DID = 992,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu20.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU20_CELL_BITMASK_DID = 993,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu21.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU21_CELL_BITMASK_DID = 994,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu22.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU22_CELL_BITMASK_DID = 995,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu23.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU23_CELL_BITMASK_DID = 996,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu24.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU24_CELL_BITMASK_DID = 997,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu25.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU25_CELL_BITMASK_DID = 998,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu26.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU26_CELL_BITMASK_DID = 999,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu27.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU27_CELL_BITMASK_DID = 1000,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu28.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU28_CELL_BITMASK_DID = 1001,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu29.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU29_CELL_BITMASK_DID = 1002,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu30.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU30_CELL_BITMASK_DID = 1003,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu31.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU31_CELL_BITMASK_DID = 1004,

            /// <summary>
            /// cdp_storage.cdp_output1
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT1_DID = 1005,

            /// <summary>
            /// cdp_storage.cdp_output2
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT2_DID = 1006,

            /// <summary>
            /// cdp_storage.cdp_output3
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT3_DID = 1007,

            /// <summary>
            /// cdp_storage.cdp_output4
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT4_DID = 1008,

            /// <summary>
            /// cdp_storage.cdp_output5
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT5_DID = 1009,

            /// <summary>
            /// cdp_storage.cdp_output6
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT6_DID = 1010,

            /// <summary>
            /// cdp_storage.cdp_output7
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT7_DID = 1011,

            /// <summary>
            /// cdp_storage.cdp_output8
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT8_DID = 1012,

            /// <summary>
            /// cdp_storage.cdp_output9
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT9_DID = 1013,

            /// <summary>
            /// cdp_storage.cdp_output10
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT10_DID = 1014,

            /// <summary>
            /// cdp_storage.cdp_output11
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT11_DID = 1015,

            /// <summary>
            /// cdp_storage.cdp_output12
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT12_DID = 1016,

            /// <summary>
            /// cdp_storage.cdp_output13
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT13_DID = 1017,

            /// <summary>
            /// cdp_storage.cdp_output14
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT14_DID = 1018,

            /// <summary>
            /// cdp_storage.cdp_output15
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT15_DID = 1019,

            /// <summary>
            /// cdp_storage.cdp_output16
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT16_DID = 1020,

            /// <summary>
            /// cdp_storage.cdp_output17
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT17_DID = 1021,

            /// <summary>
            /// cdp_storage.cdp_output18
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT18_DID = 1022,

            /// <summary>
            /// cdp_storage.cdp_output19
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT19_DID = 1023,

            /// <summary>
            /// cdp_storage.cdp_output20
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT20_DID = 1024,

            /// <summary>
            /// cdp_storage.cdp_output21
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT21_DID = 1025,

            /// <summary>
            /// cdp_storage.cdp_output22
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT22_DID = 1026,

            /// <summary>
            /// cdp_storage.cdp_output23
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT23_DID = 1027,

            /// <summary>
            /// cdp_storage.cdp_output24
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT24_DID = 1028,

            /// <summary>
            /// cdp_storage.cdp_output25
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT25_DID = 1029,

            /// <summary>
            /// task_mon_data.task_info3.id
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_ID_DID = 1030,

            /// <summary>
            /// task_mon_data.task_info3.no_of_violations
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_NO_OF_VIOLATIONS_DID = 1031,

            /// <summary>
            /// task_mon_data.task_info3.min_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_MIN_EXECUTION_TIME_MS_DID = 1032,

            /// <summary>
            /// task_mon_data.task_info3.max_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_MAX_EXECUTION_TIME_MS_DID = 1033,

            /// <summary>
            /// task_mon_data.task_info3.avg_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_AVG_EXECUTION_TIME_MS_DID = 1034,

            /// <summary>
            /// task_mon_data.task_info3.min_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_MIN_TIME_BETWEEN_EXECUTIONS_MS_DID = 1035,

            /// <summary>
            /// task_mon_data.task_info3.max_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_MAX_TIME_BETWEEN_EXECUTIONS_MS_DID = 1036,

            /// <summary>
            /// task_mon_data.task_info3.avg_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_AVG_TIME_BETWEEN_EXECUTIONS_MS_DID = 1037,

            /// <summary>
            /// task_mon_data.task_info3.min_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_MIN_EXECUTION_TIME_PCT_DID = 1038,

            /// <summary>
            /// task_mon_data.task_info3.max_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_MAX_EXECUTION_TIME_PCT_DID = 1039,

            /// <summary>
            /// task_mon_data.task_info3.avg_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_AVG_EXECUTION_TIME_PCT_DID = 1040,

            /// <summary>
            /// task_mon_data.task_info3.min_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_MIN_TIME_BETWEEN_EXECUTIONS_PCT_DID = 1041,

            /// <summary>
            /// task_mon_data.task_info3.max_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_MAX_TIME_BETWEEN_EXECUTIONS_PCT_DID = 1042,

            /// <summary>
            /// task_mon_data.task_info3.avg_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO3_AVG_TIME_BETWEEN_EXECUTIONS_PCT_DID = 1043,

            /// <summary>
            /// task_mon_data.task_info4.id
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_ID_DID = 1044,

            /// <summary>
            /// task_mon_data.task_info4.no_of_violations
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_NO_OF_VIOLATIONS_DID = 1045,

            /// <summary>
            /// task_mon_data.task_info4.min_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_MIN_EXECUTION_TIME_MS_DID = 1046,

            /// <summary>
            /// task_mon_data.task_info4.max_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_MAX_EXECUTION_TIME_MS_DID = 1047,

            /// <summary>
            /// task_mon_data.task_info4.avg_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_AVG_EXECUTION_TIME_MS_DID = 1048,

            /// <summary>
            /// task_mon_data.task_info4.min_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_MIN_TIME_BETWEEN_EXECUTIONS_MS_DID = 1049,

            /// <summary>
            /// task_mon_data.task_info4.max_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_MAX_TIME_BETWEEN_EXECUTIONS_MS_DID = 1050,

            /// <summary>
            /// task_mon_data.task_info4.avg_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_AVG_TIME_BETWEEN_EXECUTIONS_MS_DID = 1051,

            /// <summary>
            /// task_mon_data.task_info4.min_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_MIN_EXECUTION_TIME_PCT_DID = 1052,

            /// <summary>
            /// task_mon_data.task_info4.max_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_MAX_EXECUTION_TIME_PCT_DID = 1053,

            /// <summary>
            /// task_mon_data.task_info4.avg_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_AVG_EXECUTION_TIME_PCT_DID = 1054,

            /// <summary>
            /// task_mon_data.task_info4.min_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_MIN_TIME_BETWEEN_EXECUTIONS_PCT_DID = 1055,

            /// <summary>
            /// task_mon_data.task_info4.max_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_MAX_TIME_BETWEEN_EXECUTIONS_PCT_DID = 1056,

            /// <summary>
            /// task_mon_data.task_info4.avg_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO4_AVG_TIME_BETWEEN_EXECUTIONS_PCT_DID = 1057,

            /// <summary>
            /// task_mon_data.task_info5.id
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_ID_DID = 1058,

            /// <summary>
            /// task_mon_data.task_info5.no_of_violations
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_NO_OF_VIOLATIONS_DID = 1059,

            /// <summary>
            /// task_mon_data.task_info5.min_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_MIN_EXECUTION_TIME_MS_DID = 1060,

            /// <summary>
            /// task_mon_data.task_info5.max_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_MAX_EXECUTION_TIME_MS_DID = 1061,

            /// <summary>
            /// task_mon_data.task_info5.avg_execution_time_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_AVG_EXECUTION_TIME_MS_DID = 1062,

            /// <summary>
            /// task_mon_data.task_info5.min_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_MIN_TIME_BETWEEN_EXECUTIONS_MS_DID = 1063,

            /// <summary>
            /// task_mon_data.task_info5.max_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_MAX_TIME_BETWEEN_EXECUTIONS_MS_DID = 1064,

            /// <summary>
            /// task_mon_data.task_info5.avg_time_between_executions_ms
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_AVG_TIME_BETWEEN_EXECUTIONS_MS_DID = 1065,

            /// <summary>
            /// task_mon_data.task_info5.min_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_MIN_EXECUTION_TIME_PCT_DID = 1066,

            /// <summary>
            /// task_mon_data.task_info5.max_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_MAX_EXECUTION_TIME_PCT_DID = 1067,

            /// <summary>
            /// task_mon_data.task_info5.avg_execution_time_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_AVG_EXECUTION_TIME_PCT_DID = 1068,

            /// <summary>
            /// task_mon_data.task_info5.min_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_MIN_TIME_BETWEEN_EXECUTIONS_PCT_DID = 1069,

            /// <summary>
            /// task_mon_data.task_info5.max_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_MAX_TIME_BETWEEN_EXECUTIONS_PCT_DID = 1070,

            /// <summary>
            /// task_mon_data.task_info5.avg_time_between_executions_pct
            /// </summary>
            PUDS_SVC_PARAM_DI_TASK_INFO5_AVG_TIME_BETWEEN_EXECUTIONS_PCT_DID = 1071,

            /// <summary>
            /// psu_status.vdd5
            /// </summary>
            PUDS_SVC_PARAM_DI_VDD5_DID = 1072,

            /// <summary>
            /// psu_status.vdd6
            /// </summary>
            PUDS_SVC_PARAM_DI_VDD6_DID = 1073,

            /// <summary>
            /// psu_status.vsfb1
            /// </summary>
            PUDS_SVC_PARAM_DI_VSFB1_DID = 1074,

            /// <summary>
            /// psu_status.vbats
            /// </summary>
            PUDS_SVC_PARAM_DI_VBATS_DID = 1075,

            /// <summary>
            /// state_data.flags.waiting_for_staging_req
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_WAITING_FOR_STAGING_REQ_DID = 1076,

            /// <summary>
            /// pack_and_cell_data.sop_interval_data1
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_INTERVAL_DATA1_DID = 1077,

            /// <summary>
            /// pack_and_cell_data.sop_interval_data2
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_INTERVAL_DATA2_DID = 1078,

            /// <summary>
            /// pack_and_cell_data.sop_interval_data3
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_INTERVAL_DATA3_DID = 1079,

            /// <summary>
            /// pack_and_cell_data.sop_interval_data4
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_INTERVAL_DATA4_DID = 1080,

            /// <summary>
            /// pack_and_cell_data.soh
            /// </summary>
            PUDS_SVC_PARAM_DI_SOH_DID = 1081,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_V_REF2_DID = 1082,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_V_REF2_DID = 1083,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_V_REF2_DID = 1084,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_V_REF2_DID = 1085,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_V_REF2_DID = 1086,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_V_REF2_DID = 1087,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_V_REF2_DID = 1088,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_V_REF2_DID = 1089,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_V_REF2_DID = 1090,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_V_REF2_DID = 1091,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_V_REF2_DID = 1092,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_V_REF2_DID = 1093,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_V_REF2_DID = 1094,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_V_REF2_DID = 1095,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_V_REF2_DID = 1096,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_V_REF2_DID = 1097,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_V_REF2_DID = 1098,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_V_REF2_DID = 1099,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_V_REF2_DID = 1100,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_V_REF2_DID = 1101,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_V_REF2_DID = 1102,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_V_REF2_DID = 1103,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_V_REF2_DID = 1104,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_V_REF2_DID = 1105,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_V_REF2_DID = 1106,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_V_REF2_DID = 1107,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_V_REF2_DID = 1108,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_V_REF2_DID = 1109,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_V_REF2_DID = 1110,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_V_REF2_DID = 1111,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_V_REF2_DID = 1112,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_V_REF2_DID = 1113,

            /// <summary>
            /// config id
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIG_ID_DID = 1114,

            /// <summary>
            /// config crc
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIG_CRC_DID = 1115,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_DYNAMIC_SOC_0_DID = 1116,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_DYNAMIC_SOC_1_DID = 1117,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_DYNAMIC_SOC_2_DID = 1118,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_1_DID = 1119,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_2_DID = 1120,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_3_DID = 1121,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_4_DID = 1122,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_5_DID = 1123,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_6_DID = 1124,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_7_DID = 1125,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_8_DID = 1126,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_9_DID = 1127,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_10_DID = 1128,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_11_DID = 1129,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_12_DID = 1130,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_13_DID = 1131,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_14_DID = 1132,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_15_DID = 1133,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_16_DID = 1134,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_17_DID = 1135,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_18_DID = 1136,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_19_DID = 1137,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_20_DID = 1138,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_21_DID = 1139,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_22_DID = 1140,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_23_DID = 1141,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_24_DID = 1142,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_25_DID = 1143,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_26_DID = 1144,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_27_DID = 1145,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_28_DID = 1146,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_29_DID = 1147,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_30_DID = 1148,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_31_DID = 1149,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE_CMU_32_DID = 1150,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V13_DID = 1200,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V14_DID = 1201,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V15_DID = 1202,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V16_DID = 1203,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V17_DID = 1204,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V18_DID = 1205,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V13_DID = 1206,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V14_DID = 1207,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V15_DID = 1208,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V16_DID = 1209,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V17_DID = 1210,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V18_DID = 1211,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V13_DID = 1212,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V14_DID = 1213,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V15_DID = 1214,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V16_DID = 1215,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V17_DID = 1216,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V18_DID = 1217,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V13_DID = 1218,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V14_DID = 1219,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V15_DID = 1220,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V16_DID = 1221,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V17_DID = 1222,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V18_DID = 1223,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V13_DID = 1224,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V14_DID = 1225,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V15_DID = 1226,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V16_DID = 1227,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V17_DID = 1228,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V18_DID = 1229,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V13_DID = 1230,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V14_DID = 1231,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V15_DID = 1232,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V16_DID = 1233,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V17_DID = 1234,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V18_DID = 1235,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V13_DID = 1236,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V14_DID = 1237,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V15_DID = 1238,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V16_DID = 1239,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V17_DID = 1240,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V18_DID = 1241,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V13_DID = 1242,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V14_DID = 1243,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V15_DID = 1244,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V16_DID = 1245,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V17_DID = 1246,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V18_DID = 1247,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V13_DID = 1248,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V14_DID = 1249,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V15_DID = 1250,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V16_DID = 1251,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V17_DID = 1252,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V18_DID = 1253,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V13_DID = 1254,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V14_DID = 1255,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V15_DID = 1256,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V16_DID = 1257,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V17_DID = 1258,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V18_DID = 1259,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V13_DID = 1260,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V14_DID = 1261,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V15_DID = 1262,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V16_DID = 1263,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V17_DID = 1264,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V18_DID = 1265,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V13_DID = 1266,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V14_DID = 1267,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V15_DID = 1268,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V16_DID = 1269,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V17_DID = 1270,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V18_DID = 1271,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V13_DID = 1272,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V14_DID = 1273,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V15_DID = 1274,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V16_DID = 1275,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V17_DID = 1276,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V18_DID = 1277,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V13_DID = 1278,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V14_DID = 1279,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V15_DID = 1280,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V16_DID = 1281,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V17_DID = 1282,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V18_DID = 1283,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V13_DID = 1284,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V14_DID = 1285,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V15_DID = 1286,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V16_DID = 1287,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V17_DID = 1288,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V18_DID = 1289,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V13_DID = 1290,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V14_DID = 1291,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V15_DID = 1292,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V16_DID = 1293,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V17_DID = 1294,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V18_DID = 1295,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V13_DID = 1296,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V14_DID = 1297,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V15_DID = 1298,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V16_DID = 1299,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V17_DID = 1300,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V18_DID = 1301,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V13_DID = 1302,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V14_DID = 1303,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V15_DID = 1304,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V16_DID = 1305,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V17_DID = 1306,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V18_DID = 1307,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V13_DID = 1308,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V14_DID = 1309,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V15_DID = 1310,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V16_DID = 1311,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V17_DID = 1312,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V18_DID = 1313,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V13_DID = 1314,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V14_DID = 1315,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V15_DID = 1316,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V16_DID = 1317,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V17_DID = 1318,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V18_DID = 1319,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V13_DID = 1320,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V14_DID = 1321,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V15_DID = 1322,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V16_DID = 1323,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V17_DID = 1324,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V18_DID = 1325,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V13_DID = 1326,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V14_DID = 1327,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V15_DID = 1328,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V16_DID = 1329,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V17_DID = 1330,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V18_DID = 1331,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V13_DID = 1332,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V14_DID = 1333,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V15_DID = 1334,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V16_DID = 1335,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V17_DID = 1336,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V18_DID = 1337,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V13_DID = 1338,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V14_DID = 1339,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V15_DID = 1340,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V16_DID = 1341,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V17_DID = 1342,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V18_DID = 1343,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V13_DID = 1344,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V14_DID = 1345,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V15_DID = 1346,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V16_DID = 1347,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V17_DID = 1348,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V18_DID = 1349,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V13_DID = 1350,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V14_DID = 1351,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V15_DID = 1352,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V16_DID = 1353,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V17_DID = 1354,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V18_DID = 1355,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V13_DID = 1356,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V14_DID = 1357,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V15_DID = 1358,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V16_DID = 1359,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V17_DID = 1360,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V18_DID = 1361,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V13_DID = 1362,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V14_DID = 1363,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V15_DID = 1364,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V16_DID = 1365,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V17_DID = 1366,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V18_DID = 1367,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V13_DID = 1368,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V14_DID = 1369,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V15_DID = 1370,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V16_DID = 1371,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V17_DID = 1372,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V18_DID = 1373,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V13_DID = 1374,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V14_DID = 1375,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V15_DID = 1376,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V16_DID = 1377,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V17_DID = 1378,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V18_DID = 1379,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V13_DID = 1380,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V14_DID = 1381,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V15_DID = 1382,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V16_DID = 1383,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V17_DID = 1384,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V18_DID = 1385,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V13_DID = 1386,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V14_DID = 1387,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V15_DID = 1388,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V16_DID = 1389,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V17_DID = 1390,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V18_DID = 1391,

            /// <summary>
            /// pack_and_cell_data.pack_leak.neg
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_LEAK_NEG_RAW_DID = 1392,

            /// <summary>
            /// pack_and_cell_data.pack_leak.neutral
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_LEAK_MID_RAW_DID = 1393,

            /// <summary>
            /// pack_and_cell_data.pack_leak.pos
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_LEAK_POS_RAW_DID = 1394,

            /// <summary>
            /// pack_and_cell_data.pack_v.string1
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_STRING1_DID = 1395,

            /// <summary>
            /// pack_and_cell_data.pack_v.string2
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_STRING2_DID = 1396,

            /// <summary>
            /// pack_and_cell_data.pack_v.string3
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_STRING3_DID = 1397,

            /// <summary>
            /// pack_and_cell_data.pack_v.string4
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_STRING4_DID = 1398,

            /// <summary>
            /// pack_and_cell_data.pack_i.string1
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_STRING1_DID = 1411,

            /// <summary>
            /// pack_and_cell_data.pack_i.string2
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_STRING2_DID = 1412,

            /// <summary>
            /// pack_and_cell_data.pack_i.string3
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_STRING3_DID = 1413,

            /// <summary>
            /// pack_and_cell_data.pack_i.string4
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_STRING4_DID = 1414,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V1_DID = 1427,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V2_DID = 1428,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V3_DID = 1429,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V4_DID = 1430,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V5_DID = 1431,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V6_DID = 1432,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V7_DID = 1433,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V8_DID = 1434,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V9_DID = 1435,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V10_DID = 1436,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V11_DID = 1437,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V12_DID = 1438,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V13_DID = 1439,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V14_DID = 1440,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V15_DID = 1441,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V16_DID = 1442,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V17_DID = 1443,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V18_DID = 1444,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v19
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V19_DID = 1445,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v20
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V20_DID = 1446,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v21
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V21_DID = 1447,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v22
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V22_DID = 1448,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v23
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V23_DID = 1449,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v24
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V24_DID = 1450,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v25
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V25_DID = 1451,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v26
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V26_DID = 1452,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v27
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V27_DID = 1453,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v28
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V28_DID = 1454,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v29
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V29_DID = 1455,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v30
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V30_DID = 1456,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v31
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V31_DID = 1457,

            /// <summary>
            /// pack_and_cell_data.pack_v.cmu_v32
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_CMU_V32_DID = 1458,

            PUDS_SVC_PARAM_DI_Q_FULL_STRING_1_DID = 1459,
            PUDS_SVC_PARAM_DI_Q_FULL_STRING_2_DID = 1460,
            PUDS_SVC_PARAM_DI_Q_FULL_STRING_3_DID = 1461,
            PUDS_SVC_PARAM_DI_Q_FULL_STRING_4_DID = 1462,
            PUDS_SVC_PARAM_DI_Q_REMAINING_STRING_1_DID = 1463,
            PUDS_SVC_PARAM_DI_Q_REMAINING_STRING_2_DID = 1464,
            PUDS_SVC_PARAM_DI_Q_REMAINING_STRING_3_DID = 1465,
            PUDS_SVC_PARAM_DI_Q_REMAINING_STRING_4_DID = 1466,
            PUDS_SVC_PARAM_DI_SOC_STRING_1_DID = 1467,
            PUDS_SVC_PARAM_DI_SOC_STRING_2_DID = 1468,
            PUDS_SVC_PARAM_DI_SOC_STRING_3_DID = 1469,
            PUDS_SVC_PARAM_DI_SOC_STRING_4_DID = 1470,
            PUDS_SVC_PARAM_DI_SOH_STRING_1_DID = 1471,
            PUDS_SVC_PARAM_DI_SOH_STRING_2_DID = 1472,
            PUDS_SVC_PARAM_DI_SOH_STRING_3_DID = 1473,
            PUDS_SVC_PARAM_DI_SOH_STRING_4_DID = 1474,
            PUDS_SVC_PARAM_DI_DYN_LIM_I_IN_STRING_1_DID = 1475,
            PUDS_SVC_PARAM_DI_DYN_LIM_I_IN_STRING_2_DID = 1476,
            PUDS_SVC_PARAM_DI_DYN_LIM_I_IN_STRING_3_DID = 1477,
            PUDS_SVC_PARAM_DI_DYN_LIM_I_IN_STRING_4_DID = 1478,
            PUDS_SVC_PARAM_DI_DYN_LIM_I_OUT_STRING_1_DID = 1479,
            PUDS_SVC_PARAM_DI_DYN_LIM_I_OUT_STRING_2_DID = 1480,
            PUDS_SVC_PARAM_DI_DYN_LIM_I_OUT_STRING_3_DID = 1481,
            PUDS_SVC_PARAM_DI_DYN_LIM_I_OUT_STRING_4_DID = 1482,
            PUDS_SVC_PARAM_DI_SOH_MIN_DID = 1483,


            PUDS_SVC_PARAM_DI_CMU_ERROR_V_MIN_BITMASK_DID = 1484,
            PUDS_SVC_PARAM_DI_CMU_ERROR_V_MAX_BITMASK_DID = 1485,
            PUDS_SVC_PARAM_DI_CMU_ERROR_V_NO_VAL_BITMASK_DID = 1486,
            PUDS_SVC_PARAM_DI_CMU_ERROR_V_OPEN_WIRE_BITMASK_DID = 1487,
            PUDS_SVC_PARAM_DI_CMU_ERROR_T_MIN_BITMASK_DID = 1488,
            PUDS_SVC_PARAM_DI_CMU_ERROR_T_MAX_BITMASK_DID = 1489,
            PUDS_SVC_PARAM_DI_CMU_ERROR_T_NO_VAL_BITMASK_DID = 1490,
            PUDS_SVC_PARAM_DI_CMU_ERROR_T_SHORTED_VAL_BITMASK_DID = 1491,
            PUDS_SVC_PARAM_DI_CMU_ERROR_T_OPEN_VAL_BITMASK_DID = 1492,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIGURED_CMU_VERSION_DID = 3000,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_1_DID = 20001,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_2_DID = 20002,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_3_DID = 20003,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_4_DID = 20004,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_5_DID = 20005,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_6_DID = 20006,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_7_DID = 20007,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_8_DID = 20008,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_9_DID = 20009,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_10_DID = 20010,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_11_DID = 20011,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_12_DID = 20012,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_13_DID = 20013,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_14_DID = 20014,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_15_DID = 20015,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_16_DID = 20016,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_UNION_DID = 20017,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_OSC_SLOW_DID = 30001,

            /// <summary>
            ///
            /// </summary>
            PUDS_SVC_PARAM_DI_OSC_FAST_DID = 30002,

            /// <summary>
            /// can_rx_storage.can_rx_data1
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA1_DID = 50001,

            /// <summary>
            /// can_rx_storage.can_rx_data2
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA2_DID = 50002,

            /// <summary>
            /// can_rx_storage.can_rx_data3
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA3_DID = 50003,

            /// <summary>
            /// can_rx_storage.can_rx_data4
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA4_DID = 50004,

            /// <summary>
            /// can_rx_storage.can_rx_data5
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA5_DID = 50005,

            /// <summary>
            /// can_rx_storage.can_rx_data6
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA6_DID = 50006,

            /// <summary>
            /// can_rx_storage.can_rx_data7
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA7_DID = 50007,

            /// <summary>
            /// can_rx_storage.can_rx_data8
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA8_DID = 50008,

            /// <summary>
            /// can_rx_storage.can_rx_data9
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA9_DID = 50009,

            /// <summary>
            /// can_rx_storage.can_rx_data10
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA10_DID = 50010,

            /// <summary>
            /// can_rx_storage.can_rx_data11
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA11_DID = 50011,

            /// <summary>
            /// can_rx_storage.can_rx_data12
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA12_DID = 50012,

            /// <summary>
            /// can_rx_storage.can_rx_data13
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA13_DID = 50013,

            /// <summary>
            /// can_rx_storage.can_rx_data14
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA14_DID = 50014,

            /// <summary>
            /// can_rx_storage.can_rx_data15
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA15_DID = 50015,

            /// <summary>
            /// can_rx_storage.can_rx_data16
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA16_DID = 50016,

            /// <summary>
            /// can_rx_storage.can_rx_data17
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA17_DID = 50017,

            /// <summary>
            /// can_rx_storage.can_rx_data18
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA18_DID = 50018,

            /// <summary>
            /// can_rx_storage.can_rx_data19
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA19_DID = 50019,

            /// <summary>
            /// can_rx_storage.can_rx_data20
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA20_DID = 50020,

            /// <summary>
            /// can_rx_storage.can_rx_data21
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA21_DID = 50021,

            /// <summary>
            /// can_rx_storage.can_rx_data22
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA22_DID = 50022,

            /// <summary>
            /// can_rx_storage.can_rx_data23
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA23_DID = 50023,

            /// <summary>
            /// can_rx_storage.can_rx_data24
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA24_DID = 50024,

            /// <summary>
            /// can_rx_storage.can_rx_data25
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA25_DID = 50025,

            /// <summary>
            /// first_error_log_entry
            /// </summary>
            PUDS_SVC_PARAM_DI_ERROR_LOG_FIRST_ENTRY = 65000,

            /// <summary>
            /// next_error_log_entry
            /// </summary>
            PUDS_SVC_PARAM_DI_ERROR_LOG_NEXT_ENTRY = 65001,

            /// <summary>
            /// first_active_error_entry
            /// </summary>
            PUDS_SVC_PARAM_DI_ACTIVE_ERROR_FIRST_ENTRY = 65002,

            /// <summary>
            /// next_active_error_entry
            /// </summary>
            PUDS_SVC_PARAM_DI_ACTIVE_ERROR_NEXT_ENTRY = 65003,

            /// <summary>
            /// hardware_serial_number
            /// </summary>
            PUDS_SVC_PARAM_DI_HARDWARE_SERIAL_NUMBER = 65004,

            PUDS_SVC_PARAM_DI_FW_VERSION = 65005,

            /// <summary>
            /// hardware_client_serial_number
            /// </summary>
            PUDS_SVC_PARAM_DI_HARDWARE_CLIENT_SERIAL_NUMBER = 65006,


            /// <summary>
            /// bootSoftwareIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_BSIDID = 0xF180,
            /// <summary>
            /// applicationSoftwareIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ASIDID = 0xF181,
            /// <summary>
            /// applicationDataIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ADIDID = 0xF182,
            /// <summary>
            /// bootSoftwareIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_BSFPDID = 0xF183,
            /// <summary>
            /// applicationSoftwareFingerprintDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ASFPDID = 0xF184,
            /// <summary>
            /// applicationDataFingerprintDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ADFPDID = 0xF185,
            /// <summary>
            /// activeDiagnosticSessionDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ADSDID = 0xF186,
            /// <summary>
            /// vehicleManufacturerSparePartNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMSPNDID = 0xF187,
            /// <summary>
            /// vehicleManufacturerECUSoftwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMECUSNDID = 0xF188,
            /// <summary>
            /// vehicleManufacturerECUSoftwareVersionNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMECUSVNDID = 0xF189,
            /// <summary>
            /// systemSupplierIdentifierDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSIDDID = 0xF18A,
            /// <summary>
            /// ECUManufacturingDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ECUMDDID = 0xF18B,
            /// <summary>
            /// ECUSerialNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ECUSNDID = 0xF18C,
            /// <summary>
            /// supportedFunctionalUnitsDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SFUDID = 0xF18D,
            /// <summary>
            /// vehicleManufacturerKitAssemblyPartNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMKAPNDID = 0xF18E,
            /// <summary>
            /// VINDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VINDID = 0xF190,
            /// <summary>
            /// vehicleManufacturerECUHardwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMECUHNDID = 0xF191,
            /// <summary>
            /// systemSupplierECUHardwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUHWNDID = 0xF192,
            /// <summary>
            /// systemSupplierECUHardwareVersionNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUHWVNDID = 0xF193,
            /// <summary>
            /// systemSupplierECUSoftwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUSWNDID = 0xF194,
            /// <summary>
            /// systemSupplierECUSoftwareVersionNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUSWVNDID = 0xF195,
            /// <summary>
            /// exhaustRegulationOrTypeApprovalNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_EROTANDID = 0xF196,
            /// <summary>
            /// systemNameOrEngineTypeDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SNOETDID = 0xF197,
            /// <summary>
            /// repairShopCodeOrTesterSerialNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_RSCOTSNDID = 0xF198,
            /// <summary>
            /// programmingDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_PDDID = 0xF199,
            /// <summary>
            /// calibrationRepairShopCodeOrCalibrationEquipmentSerialNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_CRSCOCESNDID = 0xF19A,
            /// <summary>
            /// calibrationDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_CDDID = 0xF19B,
            /// <summary>
            /// calibrationEquipmentSoftwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_CESWNDID = 0xF19C,
            /// <summary>
            /// ECUInstallationDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_EIDDID = 0xF19D,
            /// <summary>
            /// ODXFileDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ODXFDID = 0xF19E,
            /// <summary>
            /// entityDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_EDID = 0xF19F,
        }

        public enum TPUDSSvcParamDICIP : ushort
        {
            /// <summary>
            /// pack_and_cell_data.cell_v.min.val
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MIN_VAL_DID = 1,

            /// <summary>
            /// pack_and_cell_data.cell_v.min.id.cmu
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MIN_ID_CMU_DID = 2,

            /// <summary>
            /// pack_and_cell_data.cell_v.min.id.cell
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MIN_ID_CELL_DID = 3,

            /// <summary>
            /// pack_and_cell_data.cell_v.max.val
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MAX_VAL_DID = 4,

            /// <summary>
            /// pack_and_cell_data.cell_v.max.id.cmu
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MAX_ID_CMU_DID = 5,

            /// <summary>
            /// pack_and_cell_data.cell_v.max.id.cell
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_MAX_ID_CELL_DID = 6,

            /// <summary>
            /// pack_and_cell_data.cell_v.avg
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_AVG_DID = 7,

            /// <summary>
            /// pack_and_cell_data.cell_v.num_available
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_V_NUM_AVAILABLE_DID = 8,

            /// <summary>
            /// pack_and_cell_data.pack_v.hv_1
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_HV_1_DID = 9,

            /// <summary>
            /// pack_and_cell_data.pack_v.hv_2
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_HV_2_DID = 10,

            /// <summary>
            /// pack_and_cell_data.pack_v.sum_of_cells
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_V_SUM_OF_CELLS_DID = 11,

            /// <summary>
            /// pack_and_cell_data.hall_v.hall_v_lo
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_V_HALL_V_LO_DID = 12,

            /// <summary>
            /// pack_and_cell_data.hall_v.hall_v_hi
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_V_HALL_V_HI_DID = 13,

            /// <summary>
            /// pack_and_cell_data.pack_i.hall
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_HALL_DID = 14,

            /// <summary>
            /// pack_and_cell_data.pack_i.shunt
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_SHUNT_DID = 15,

            /// <summary>
            /// pack_and_cell_data.pack_i.master
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_I_MASTER_DID = 16,

            /// <summary>
            /// pack_and_cell_data.pack_q.remaining_hi_res
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_REMAINING_HI_RES_DID = 17,

            /// <summary>
            /// soc_internal
            /// </summary>
            PUDS_SVC_PARAM_DI_SOC_INTERNAL_DID = 18,

            /// <summary>
            /// soc_trimmed
            /// </summary>
            PUDS_SVC_PARAM_DI_SOC_TRIMMED_DID = 19,

            /// <summary>
            /// pack_and_cell_data.pack_q.remaining_nominal
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_REMAINING_NOMINAL_DID = 20,

            /// <summary>
            /// pack_and_cell_data.pack_q.design
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_DESIGN_DID = 21,

            /// <summary>
            /// pack_and_cell_data.pack_q.full
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_Q_FULL_DID = 22,

            /// <summary>
            /// state_data.io_state.output
            /// </summary>
            PUDS_SVC_PARAM_DI_IO_STATE_OUTPUT_DID = 23,

            /// <summary>
            /// state_data.io_state.input
            /// </summary>
            PUDS_SVC_PARAM_DI_IO_STATE_INPUT_DID = 24,

            /// <summary>
            /// state_data.charger.output_enabled
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_OUTPUT_ENABLED_DID = 25,

            /// <summary>
            /// state_data.charger.output_current
            /// </summary>
            PUDS_SVC_PARAM_DI_REQUESTED_CHARGER_CURRENT_DID = 26,

            /// <summary>
            /// state_data.charger.output_voltage
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_OUTPUT_VOLTAGE_DID = 27,

            /// <summary>
            /// state_data.charger.can_active
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_CAN_ACTIVE_DID = 28,

            /// <summary>
            /// state_data.charger.pwm_active
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_PWM_ACTIVE_DID = 29,

            /// <summary>
            /// state_data.charger.pwm_active_duty
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_PWM_ACTIVE_DUTY_DID = 30,

            /// <summary>
            /// i2t_remain_discharge
            /// </summary>
            PUDS_SVC_PARAM_DI_I2T_REMAIN_DISCHARGE_DID = 31,

            /// <summary>
            /// dcli
            /// </summary>
            PUDS_SVC_PARAM_DI_DCLI_DID = 32,

            /// <summary>
            /// dclo
            /// </summary>
            PUDS_SVC_PARAM_DI_DCLO_DID = 33,

            /// <summary>
            /// state_data.status
            /// </summary>
            PUDS_SVC_PARAM_DI_STATUS_DID = 34,

            /// <summary>
            /// state_data.contactors.enabled
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ENABLED_DID = 35,

            /// <summary>
            /// state_data.contactors.activate_charge
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ACTIVATE_CHARGE_DID = 36,

            /// <summary>
            /// state_data.contactors.activate_load
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ACTIVATE_LOAD_DID = 37,

            /// <summary>
            /// state_data.contactors.activate_combined
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ACTIVATE_COMBINED_DID = 38,

            /// <summary>
            /// state_data.contactors.charger_activated
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_CHARGER_ACTIVATED_DID = 39,

            /// <summary>
            /// state_data.contactors.load_activated
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_LOAD_ACTIVATED_DID = 40,

            /// <summary>
            /// state_data.contactors.combined_activated
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_COMBINED_ACTIVATED_DID = 41,

            /// <summary>
            /// state_data.contactors.activation_allowed
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_ACTIVATION_ALLOWED_DID = 42,

            /// <summary>
            /// state_data.contactors.emergency_stop
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_EMERGENCY_STOP_DID = 43,

            /// <summary>
            /// state_data.contactors.contactor_retries
            /// </summary>
            PUDS_SVC_PARAM_DI_CONTACTORS_CONTACTOR_RETRIES_DID = 44,

            /// <summary>
            /// state_data.balancing.allowed
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_ALLOWED_DID = 45,

            /// <summary>
            /// state_data.balancing.limit
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_LIMIT_DID = 46,

            /// <summary>
            /// state_data.flags.fully_charged
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_FULLY_CHARGED_DID = 47,

            /// <summary>
            /// state_data.flags.fully_charged_latched
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_FULLY_CHARGED_LATCHED_DID = 48,

            /// <summary>
            /// state_data.flags.load_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_LOAD_ACTIVE_DID = 49,

            /// <summary>
            /// state_data.flags.charger_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_CHARGER_ACTIVE_DID = 50,

            /// <summary>
            /// state_data.flags.precharge_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_PRECHARGE_ACTIVE_DID = 51,

            /// <summary>
            /// state_data.flags.balancing_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_BALANCING_ACTIVE_DID = 52,

            /// <summary>
            /// state_data.flags.charge_reg_active
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_CHARGE_REG_ACTIVE_DID = 53,

            /// <summary>
            /// psu_status.b_wu_ign
            /// </summary>
            PUDS_SVC_PARAM_DI_B_WU_IGN_DID = 54,

            /// <summary>
            /// psu_status.b_wu_can
            /// </summary>
            PUDS_SVC_PARAM_DI_B_WU_CAN_DID = 55,

            /// <summary>
            /// psu_status.vcp
            /// </summary>
            PUDS_SVC_PARAM_DI_VCP_DID = 56,

            /// <summary>
            /// psu_status.vbat
            /// </summary>
            PUDS_SVC_PARAM_DI_VBAT_DID = 57,

            /// <summary>
            /// psu_status.main
            /// </summary>
            PUDS_SVC_PARAM_DI_MAIN_DID = 58,

            /// <summary>
            /// psu_status.vmon
            /// </summary>
            PUDS_SVC_PARAM_DI_VMON_DID = 59,

            /// <summary>
            /// error_status.critical_severity_count
            /// </summary>
            PUDS_SVC_PARAM_DI_CRITICAL_SEVERITY_COUNT_DID = 60,

            /// <summary>
            /// error_status.normal_severity_count
            /// </summary>
            PUDS_SVC_PARAM_DI_NORMAL_SEVERITY_COUNT_DID = 61,

            /// <summary>
            /// error_status.low_severity_count
            /// </summary>
            PUDS_SVC_PARAM_DI_LOW_SEVERITY_COUNT_DID = 62,

            /// <summary>
            /// error_status.active_count
            /// </summary>
            PUDS_SVC_PARAM_DI_ACTIVE_COUNT_DID = 63,

            /// <summary>
            /// version_entry.project
            /// </summary>
            PUDS_SVC_PARAM_DI_PROJECT_DID = 64,

            /// <summary>
            /// version_entry.hw_pcb
            /// </summary>
            PUDS_SVC_PARAM_DI_HW_PCB_DID = 65,

            /// <summary>
            /// version_entry.hw_bom
            /// </summary>
            PUDS_SVC_PARAM_DI_HW_BOM_DID = 66,

            /// <summary>
            /// version_entry.fw_major
            /// </summary>
            PUDS_SVC_PARAM_DI_FW_MAJOR_DID = 67,

            /// <summary>
            /// version_entry.fw_minor
            /// </summary>
            PUDS_SVC_PARAM_DI_FW_MINOR_DID = 68,

            /// <summary>
            /// version_entry.type
            /// </summary>
            PUDS_SVC_PARAM_DI_TYPE_DID = 69,

            /// <summary>
            /// version_entry.build_date
            /// </summary>
            PUDS_SVC_PARAM_DI_BUILD_DATE_DID = 70,

            /// <summary>
            /// version_entry.build_time
            /// </summary>
            PUDS_SVC_PARAM_DI_BUILD_TIME_DID = 71,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_BALANCE_T1_DID = 72,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_BALANCE_T2_DID = 73,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_BALANCE_T1_DID = 74,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_BALANCE_T2_DID = 75,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_BALANCE_T1_DID = 76,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_BALANCE_T2_DID = 77,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_BALANCE_T1_DID = 78,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_BALANCE_T2_DID = 79,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_BALANCE_T1_DID = 80,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_BALANCE_T2_DID = 81,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_BALANCE_T1_DID = 82,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_BALANCE_T2_DID = 83,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_BALANCE_T1_DID = 84,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_BALANCE_T2_DID = 85,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_BALANCE_T1_DID = 86,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_BALANCE_T2_DID = 87,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_BALANCE_T1_DID = 88,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_BALANCE_T2_DID = 89,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_BALANCE_T1_DID = 90,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_BALANCE_T2_DID = 91,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_BALANCE_T1_DID = 92,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_BALANCE_T2_DID = 93,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_BALANCE_T1_DID = 94,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_BALANCE_T2_DID = 95,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_BALANCE_T1_DID = 96,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_BALANCE_T2_DID = 97,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_BALANCE_T1_DID = 98,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_BALANCE_T2_DID = 99,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_BALANCE_T1_DID = 100,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_BALANCE_T2_DID = 101,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_BALANCE_T1_DID = 102,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_BALANCE_T2_DID = 103,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_BALANCE_T1_DID = 104,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_BALANCE_T2_DID = 105,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_BALANCE_T1_DID = 106,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_BALANCE_T2_DID = 107,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_BALANCE_T1_DID = 108,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_BALANCE_T2_DID = 109,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_BALANCE_T1_DID = 110,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_BALANCE_T2_DID = 111,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_BALANCE_T1_DID = 112,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_BALANCE_T2_DID = 113,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_BALANCE_T1_DID = 114,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_BALANCE_T2_DID = 115,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_BALANCE_T1_DID = 116,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_BALANCE_T2_DID = 117,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_BALANCE_T1_DID = 118,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_BALANCE_T2_DID = 119,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_BALANCE_T1_DID = 120,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_BALANCE_T2_DID = 121,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_BALANCE_T1_DID = 122,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_BALANCE_T2_DID = 123,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_BALANCE_T1_DID = 124,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_BALANCE_T2_DID = 125,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_BALANCE_T1_DID = 126,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_BALANCE_T2_DID = 127,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_BALANCE_T1_DID = 128,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_BALANCE_T2_DID = 129,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_BALANCE_T1_DID = 130,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_BALANCE_T2_DID = 131,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_BALANCE_T1_DID = 132,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_BALANCE_T2_DID = 133,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.balance_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_BALANCE_T1_DID = 134,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.balance_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_BALANCE_T2_DID = 135,

            /// <summary>
            /// pack_and_cell_data.pcb_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T1_DID = 136,

            /// <summary>
            /// pack_and_cell_data.pcb_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_PCB_T2_DID = 137,

            /// <summary>
            /// pack_and_cell_data.mcu_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T1_DID = 138,

            /// <summary>
            /// pack_and_cell_data.mcu_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T2_DID = 139,

            /// <summary>
            /// pack_and_cell_data.mcu_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T3_DID = 140,

            /// <summary>
            /// pack_and_cell_data.mcu_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T4_DID = 141,

            /// <summary>
            /// pack_and_cell_data.mcu_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T5_DID = 142,

            /// <summary>
            /// pack_and_cell_data.mcu_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T6_DID = 143,

            /// <summary>
            /// pack_and_cell_data.mcu_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T7_DID = 144,

            /// <summary>
            /// pack_and_cell_data.mcu_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T8_DID = 145,

            /// <summary>
            /// pack_and_cell_data.mcu_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T9_DID = 146,

            /// <summary>
            /// pack_and_cell_data.mcu_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T10_DID = 147,

            /// <summary>
            /// pack_and_cell_data.mcu_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_T11_DID = 148,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T1_DID = 149,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T2_DID = 150,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T3_DID = 151,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T4_DID = 152,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T5_DID = 153,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T6_DID = 154,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T7_DID = 155,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T8_DID = 156,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T9_DID = 157,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T10_DID = 158,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T11_DID = 159,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_T12_DID = 160,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T1_DID = 161,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T2_DID = 162,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T3_DID = 163,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T4_DID = 164,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T5_DID = 165,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T6_DID = 166,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T7_DID = 167,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T8_DID = 168,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T9_DID = 169,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T10_DID = 170,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T11_DID = 171,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_T12_DID = 172,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T1_DID = 173,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T2_DID = 174,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T3_DID = 175,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T4_DID = 176,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T5_DID = 177,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T6_DID = 178,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T7_DID = 179,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T8_DID = 180,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T9_DID = 181,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T10_DID = 182,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T11_DID = 183,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_T12_DID = 184,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T1_DID = 185,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T2_DID = 186,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T3_DID = 187,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T4_DID = 188,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T5_DID = 189,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T6_DID = 190,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T7_DID = 191,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T8_DID = 192,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T9_DID = 193,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T10_DID = 194,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T11_DID = 195,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_T12_DID = 196,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T1_DID = 197,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T2_DID = 198,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T3_DID = 199,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T4_DID = 200,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T5_DID = 201,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T6_DID = 202,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T7_DID = 203,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T8_DID = 204,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T9_DID = 205,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T10_DID = 206,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T11_DID = 207,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_T12_DID = 208,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T1_DID = 209,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T2_DID = 210,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T3_DID = 211,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T4_DID = 212,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T5_DID = 213,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T6_DID = 214,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T7_DID = 215,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T8_DID = 216,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T9_DID = 217,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T10_DID = 218,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T11_DID = 219,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_T12_DID = 220,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T1_DID = 221,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T2_DID = 222,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T3_DID = 223,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T4_DID = 224,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T5_DID = 225,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T6_DID = 226,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T7_DID = 227,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T8_DID = 228,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T9_DID = 229,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T10_DID = 230,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T11_DID = 231,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_T12_DID = 232,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T1_DID = 233,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T2_DID = 234,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T3_DID = 235,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T4_DID = 236,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T5_DID = 237,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T6_DID = 238,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T7_DID = 239,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T8_DID = 240,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T9_DID = 241,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T10_DID = 242,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T11_DID = 243,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_T12_DID = 244,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T1_DID = 245,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T2_DID = 246,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T3_DID = 247,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T4_DID = 248,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T5_DID = 249,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T6_DID = 250,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T7_DID = 251,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T8_DID = 252,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T9_DID = 253,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T10_DID = 254,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T11_DID = 255,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_T12_DID = 256,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T1_DID = 257,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T2_DID = 258,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T3_DID = 259,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T4_DID = 260,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T5_DID = 261,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T6_DID = 262,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T7_DID = 263,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T8_DID = 264,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T9_DID = 265,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T10_DID = 266,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T11_DID = 267,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_T12_DID = 268,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T1_DID = 269,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T2_DID = 270,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T3_DID = 271,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T4_DID = 272,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T5_DID = 273,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T6_DID = 274,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T7_DID = 275,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T8_DID = 276,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T9_DID = 277,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T10_DID = 278,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T11_DID = 279,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_T12_DID = 280,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T1_DID = 281,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T2_DID = 282,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T3_DID = 283,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T4_DID = 284,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T5_DID = 285,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T6_DID = 286,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T7_DID = 287,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T8_DID = 288,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T9_DID = 289,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T10_DID = 290,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T11_DID = 291,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_T12_DID = 292,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T1_DID = 293,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T2_DID = 294,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T3_DID = 295,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T4_DID = 296,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T5_DID = 297,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T6_DID = 298,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T7_DID = 299,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T8_DID = 300,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T9_DID = 301,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T10_DID = 302,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T11_DID = 303,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_T12_DID = 304,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T1_DID = 305,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T2_DID = 306,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T3_DID = 307,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T4_DID = 308,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T5_DID = 309,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T6_DID = 310,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T7_DID = 311,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T8_DID = 312,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T9_DID = 313,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T10_DID = 314,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T11_DID = 315,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_T12_DID = 316,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T1_DID = 317,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T2_DID = 318,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T3_DID = 319,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T4_DID = 320,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T5_DID = 321,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T6_DID = 322,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T7_DID = 323,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T8_DID = 324,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T9_DID = 325,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T10_DID = 326,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T11_DID = 327,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_T12_DID = 328,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T1_DID = 329,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T2_DID = 330,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T3_DID = 331,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T4_DID = 332,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T5_DID = 333,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T6_DID = 334,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T7_DID = 335,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T8_DID = 336,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T9_DID = 337,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T10_DID = 338,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T11_DID = 339,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_T12_DID = 340,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T1_DID = 341,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T2_DID = 342,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T3_DID = 343,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T4_DID = 344,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T5_DID = 345,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T6_DID = 346,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T7_DID = 347,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T8_DID = 348,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T9_DID = 349,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T10_DID = 350,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T11_DID = 351,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_T12_DID = 352,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T1_DID = 353,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T2_DID = 354,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T3_DID = 355,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T4_DID = 356,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T5_DID = 357,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T6_DID = 358,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T7_DID = 359,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T8_DID = 360,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T9_DID = 361,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T10_DID = 362,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T11_DID = 363,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_T12_DID = 364,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T1_DID = 365,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T2_DID = 366,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T3_DID = 367,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T4_DID = 368,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T5_DID = 369,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T6_DID = 370,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T7_DID = 371,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T8_DID = 372,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T9_DID = 373,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T10_DID = 374,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T11_DID = 375,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_T12_DID = 376,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T1_DID = 377,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T2_DID = 378,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T3_DID = 379,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T4_DID = 380,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T5_DID = 381,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T6_DID = 382,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T7_DID = 383,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T8_DID = 384,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T9_DID = 385,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T10_DID = 386,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T11_DID = 387,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_T12_DID = 388,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T1_DID = 389,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T2_DID = 390,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T3_DID = 391,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T4_DID = 392,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T5_DID = 393,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T6_DID = 394,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T7_DID = 395,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T8_DID = 396,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T9_DID = 397,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T10_DID = 398,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T11_DID = 399,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_T12_DID = 400,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T1_DID = 401,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T2_DID = 402,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T3_DID = 403,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T4_DID = 404,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T5_DID = 405,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T6_DID = 406,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T7_DID = 407,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T8_DID = 408,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T9_DID = 409,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T10_DID = 410,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T11_DID = 411,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_T12_DID = 412,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T1_DID = 413,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T2_DID = 414,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T3_DID = 415,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T4_DID = 416,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T5_DID = 417,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T6_DID = 418,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T7_DID = 419,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T8_DID = 420,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T9_DID = 421,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T10_DID = 422,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T11_DID = 423,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_T12_DID = 424,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T1_DID = 425,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T2_DID = 426,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T3_DID = 427,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T4_DID = 428,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T5_DID = 429,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T6_DID = 430,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T7_DID = 431,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T8_DID = 432,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T9_DID = 433,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T10_DID = 434,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T11_DID = 435,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_T12_DID = 436,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T1_DID = 437,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T2_DID = 438,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T3_DID = 439,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T4_DID = 440,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T5_DID = 441,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T6_DID = 442,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T7_DID = 443,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T8_DID = 444,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T9_DID = 445,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T10_DID = 446,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T11_DID = 447,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_T12_DID = 448,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T1_DID = 449,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T2_DID = 450,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T3_DID = 451,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T4_DID = 452,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T5_DID = 453,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T6_DID = 454,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T7_DID = 455,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T8_DID = 456,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T9_DID = 457,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T10_DID = 458,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T11_DID = 459,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_T12_DID = 460,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T1_DID = 461,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T2_DID = 462,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T3_DID = 463,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T4_DID = 464,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T5_DID = 465,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T6_DID = 466,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T7_DID = 467,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T8_DID = 468,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T9_DID = 469,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T10_DID = 470,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T11_DID = 471,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_T12_DID = 472,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T1_DID = 473,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T2_DID = 474,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T3_DID = 475,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T4_DID = 476,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T5_DID = 477,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T6_DID = 478,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T7_DID = 479,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T8_DID = 480,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T9_DID = 481,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T10_DID = 482,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T11_DID = 483,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_T12_DID = 484,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T1_DID = 485,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T2_DID = 486,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T3_DID = 487,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T4_DID = 488,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T5_DID = 489,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T6_DID = 490,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T7_DID = 491,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T8_DID = 492,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T9_DID = 493,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T10_DID = 494,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T11_DID = 495,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_T12_DID = 496,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T1_DID = 497,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T2_DID = 498,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T3_DID = 499,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T4_DID = 500,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T5_DID = 501,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T6_DID = 502,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T7_DID = 503,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T8_DID = 504,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T9_DID = 505,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T10_DID = 506,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T11_DID = 507,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_T12_DID = 508,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T1_DID = 509,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T2_DID = 510,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T3_DID = 511,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T4_DID = 512,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T5_DID = 513,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T6_DID = 514,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T7_DID = 515,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T8_DID = 516,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T9_DID = 517,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T10_DID = 518,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T11_DID = 519,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_T12_DID = 520,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T1_DID = 521,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T2_DID = 522,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T3_DID = 523,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T4_DID = 524,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T5_DID = 525,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T6_DID = 526,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T7_DID = 527,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T8_DID = 528,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T9_DID = 529,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T10_DID = 530,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T11_DID = 531,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_t12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_T12_DID = 532,

            /// <summary>
            /// pack_and_cell_data.mcu_test_t1
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_TEST_T1_DID = 533,

            /// <summary>
            /// pack_and_cell_data.mcu_test_t2
            /// </summary>
            PUDS_SVC_PARAM_DI_MCU_TEST_T2_DID = 534,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.v_ref_5v
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_V_REF_5V_DID = 535,

            /// <summary>
            /// online_count
            /// </summary>
            PUDS_SVC_PARAM_DI_ONLINE_COUNT_DID = 538,

            /// <summary>
            /// pack_and_cell_data.cmus.comm_err
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_COMM_ERR_DID = 539,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V1_DID = 540,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V2_DID = 541,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V3_DID = 542,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V4_DID = 543,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V5_DID = 544,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V6_DID = 545,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V7_DID = 546,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V8_DID = 547,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V9_DID = 548,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V10_DID = 549,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V11_DID = 550,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V12_DID = 551,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V1_DID = 552,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V2_DID = 553,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V3_DID = 554,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V4_DID = 555,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V5_DID = 556,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V6_DID = 557,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V7_DID = 558,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V8_DID = 559,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V9_DID = 560,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V10_DID = 561,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V11_DID = 562,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V12_DID = 563,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V1_DID = 564,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V2_DID = 565,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V3_DID = 566,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V4_DID = 567,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V5_DID = 568,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V6_DID = 569,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V7_DID = 570,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V8_DID = 571,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V9_DID = 572,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V10_DID = 573,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V11_DID = 574,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V12_DID = 575,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V1_DID = 576,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V2_DID = 577,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V3_DID = 578,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V4_DID = 579,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V5_DID = 580,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V6_DID = 581,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V7_DID = 582,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V8_DID = 583,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V9_DID = 584,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V10_DID = 585,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V11_DID = 586,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V12_DID = 587,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V1_DID = 588,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V2_DID = 589,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V3_DID = 590,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V4_DID = 591,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V5_DID = 592,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V6_DID = 593,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V7_DID = 594,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V8_DID = 595,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V9_DID = 596,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V10_DID = 597,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V11_DID = 598,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V12_DID = 599,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V1_DID = 600,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V2_DID = 601,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V3_DID = 602,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V4_DID = 603,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V5_DID = 604,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V6_DID = 605,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V7_DID = 606,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V8_DID = 607,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V9_DID = 608,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V10_DID = 609,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V11_DID = 610,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V12_DID = 611,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V1_DID = 612,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V2_DID = 613,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V3_DID = 614,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V4_DID = 615,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V5_DID = 616,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V6_DID = 617,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V7_DID = 618,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V8_DID = 619,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V9_DID = 620,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V10_DID = 621,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V11_DID = 622,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V12_DID = 623,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V1_DID = 624,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V2_DID = 625,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V3_DID = 626,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V4_DID = 627,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V5_DID = 628,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V6_DID = 629,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V7_DID = 630,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V8_DID = 631,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V9_DID = 632,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V10_DID = 633,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V11_DID = 634,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V12_DID = 635,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V1_DID = 636,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V2_DID = 637,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V3_DID = 638,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V4_DID = 639,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V5_DID = 640,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V6_DID = 641,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V7_DID = 642,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V8_DID = 643,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V9_DID = 644,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V10_DID = 645,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V11_DID = 646,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V12_DID = 647,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V1_DID = 648,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V2_DID = 649,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V3_DID = 650,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V4_DID = 651,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V5_DID = 652,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V6_DID = 653,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V7_DID = 654,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V8_DID = 655,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V9_DID = 656,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V10_DID = 657,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V11_DID = 658,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V12_DID = 659,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V1_DID = 660,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V2_DID = 661,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V3_DID = 662,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V4_DID = 663,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V5_DID = 664,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V6_DID = 665,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V7_DID = 666,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V8_DID = 667,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V9_DID = 668,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V10_DID = 669,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V11_DID = 670,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V12_DID = 671,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V1_DID = 672,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V2_DID = 673,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V3_DID = 674,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V4_DID = 675,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V5_DID = 676,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V6_DID = 677,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V7_DID = 678,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V8_DID = 679,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V9_DID = 680,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V10_DID = 681,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V11_DID = 682,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V12_DID = 683,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V1_DID = 684,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V2_DID = 685,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V3_DID = 686,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V4_DID = 687,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V5_DID = 688,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V6_DID = 689,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V7_DID = 690,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V8_DID = 691,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V9_DID = 692,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V10_DID = 693,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V11_DID = 694,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V12_DID = 695,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V1_DID = 696,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V2_DID = 697,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V3_DID = 698,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V4_DID = 699,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V5_DID = 700,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V6_DID = 701,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V7_DID = 702,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V8_DID = 703,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V9_DID = 704,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V10_DID = 705,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V11_DID = 706,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V12_DID = 707,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V1_DID = 708,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V2_DID = 709,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V3_DID = 710,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V4_DID = 711,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V5_DID = 712,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V6_DID = 713,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V7_DID = 714,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V8_DID = 715,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V9_DID = 716,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V10_DID = 717,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V11_DID = 718,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V12_DID = 719,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V1_DID = 720,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V2_DID = 721,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V3_DID = 722,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V4_DID = 723,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V5_DID = 724,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V6_DID = 725,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V7_DID = 726,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V8_DID = 727,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V9_DID = 728,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V10_DID = 729,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V11_DID = 730,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V12_DID = 731,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V1_DID = 732,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V2_DID = 733,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V3_DID = 734,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V4_DID = 735,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V5_DID = 736,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V6_DID = 737,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V7_DID = 738,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V8_DID = 739,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V9_DID = 740,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V10_DID = 741,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V11_DID = 742,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V12_DID = 743,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V1_DID = 744,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V2_DID = 745,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V3_DID = 746,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V4_DID = 747,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V5_DID = 748,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V6_DID = 749,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V7_DID = 750,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V8_DID = 751,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V9_DID = 752,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V10_DID = 753,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V11_DID = 754,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V12_DID = 755,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V1_DID = 756,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V2_DID = 757,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V3_DID = 758,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V4_DID = 759,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V5_DID = 760,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V6_DID = 761,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V7_DID = 762,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V8_DID = 763,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V9_DID = 764,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V10_DID = 765,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V11_DID = 766,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V12_DID = 767,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V1_DID = 768,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V2_DID = 769,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V3_DID = 770,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V4_DID = 771,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V5_DID = 772,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V6_DID = 773,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V7_DID = 774,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V8_DID = 775,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V9_DID = 776,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V10_DID = 777,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V11_DID = 778,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V12_DID = 779,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V1_DID = 780,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V2_DID = 781,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V3_DID = 782,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V4_DID = 783,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V5_DID = 784,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V6_DID = 785,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V7_DID = 786,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V8_DID = 787,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V9_DID = 788,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V10_DID = 789,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V11_DID = 790,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V12_DID = 791,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V1_DID = 792,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V2_DID = 793,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V3_DID = 794,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V4_DID = 795,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V5_DID = 796,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V6_DID = 797,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V7_DID = 798,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V8_DID = 799,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V9_DID = 800,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V10_DID = 801,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V11_DID = 802,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V12_DID = 803,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V1_DID = 804,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V2_DID = 805,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V3_DID = 806,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V4_DID = 807,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V5_DID = 808,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V6_DID = 809,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V7_DID = 810,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V8_DID = 811,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V9_DID = 812,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V10_DID = 813,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V11_DID = 814,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V12_DID = 815,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V1_DID = 816,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V2_DID = 817,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V3_DID = 818,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V4_DID = 819,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V5_DID = 820,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V6_DID = 821,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V7_DID = 822,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V8_DID = 823,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V9_DID = 824,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V10_DID = 825,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V11_DID = 826,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V12_DID = 827,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V1_DID = 828,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V2_DID = 829,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V3_DID = 830,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V4_DID = 831,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V5_DID = 832,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V6_DID = 833,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V7_DID = 834,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V8_DID = 835,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V9_DID = 836,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V10_DID = 837,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V11_DID = 838,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V12_DID = 839,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V1_DID = 840,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V2_DID = 841,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V3_DID = 842,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V4_DID = 843,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V5_DID = 844,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V6_DID = 845,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V7_DID = 846,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V8_DID = 847,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V9_DID = 848,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V10_DID = 849,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V11_DID = 850,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V12_DID = 851,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V1_DID = 852,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V2_DID = 853,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V3_DID = 854,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V4_DID = 855,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V5_DID = 856,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V6_DID = 857,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V7_DID = 858,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V8_DID = 859,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V9_DID = 860,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V10_DID = 861,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V11_DID = 862,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V12_DID = 863,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V1_DID = 864,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V2_DID = 865,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V3_DID = 866,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V4_DID = 867,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V5_DID = 868,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V6_DID = 869,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V7_DID = 870,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V8_DID = 871,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V9_DID = 872,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V10_DID = 873,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V11_DID = 874,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V12_DID = 875,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V1_DID = 876,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V2_DID = 877,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V3_DID = 878,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V4_DID = 879,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V5_DID = 880,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V6_DID = 881,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V7_DID = 882,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V8_DID = 883,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V9_DID = 884,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V10_DID = 885,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V11_DID = 886,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V12_DID = 887,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V1_DID = 888,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V2_DID = 889,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V3_DID = 890,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V4_DID = 891,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V5_DID = 892,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V6_DID = 893,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V7_DID = 894,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V8_DID = 895,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V9_DID = 896,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V10_DID = 897,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V11_DID = 898,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V12_DID = 899,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V1_DID = 900,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V2_DID = 901,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V3_DID = 902,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V4_DID = 903,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V5_DID = 904,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V6_DID = 905,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V7_DID = 906,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V8_DID = 907,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V9_DID = 908,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V10_DID = 909,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V11_DID = 910,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V12_DID = 911,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v1
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V1_DID = 912,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V2_DID = 913,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v3
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V3_DID = 914,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v4
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V4_DID = 915,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v5
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V5_DID = 916,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v6
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V6_DID = 917,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v7
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V7_DID = 918,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v8
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V8_DID = 919,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v9
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V9_DID = 920,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v10
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V10_DID = 921,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v11
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V11_DID = 922,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v12
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V12_DID = 923,

            /// <summary>
            /// reserved924
            /// </summary>
            PUDS_SVC_PARAM_DI_RESERVED924_DID = 924,

            /// <summary>
            /// reserved953
            /// </summary>
            PUDS_SVC_PARAM_DI_RESERVED953_DID = 953,

            /// <summary>
            /// reserved954
            /// </summary>
            PUDS_SVC_PARAM_DI_RESERVED954_DID = 954,

            /// <summary>
            /// time_between_boots_s
            /// </summary>
            PUDS_SVC_PARAM_DI_TIME_BETWEEN_BOOTS_S_DID = 955,

            /// <summary>
            /// epoch_time_s
            /// </summary>
            PUDS_SVC_PARAM_DI_EPOCH_TIME_S_DID = 956,

            /// <summary>
            /// uptime_s
            /// </summary>
            PUDS_SVC_PARAM_DI_UPTIME_S_DID = 957,

            /// <summary>
            /// pack_and_cell_data.hall_ext_v.hall_v_lo
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_EXT_V_HALL_V_LO_DID = 958,

            /// <summary>
            /// pack_and_cell_data.hall_ext_v.hall_v_hi
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_EXT_V_HALL_V_HI_DID = 959,

            /// <summary>
            /// pack_and_cell_data.hall_ext_v.hall_v_supply
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_EXT_V_HALL_V_SUPPLY_DID = 960,

            /// <summary>
            /// pack_and_cell_data.hall_sys_v.hall_v_lo
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_SYS_V_HALL_V_LO_DID = 961,

            /// <summary>
            /// pack_and_cell_data.hall_sys_v.hall_v_hi
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_SYS_V_HALL_V_HI_DID = 962,

            /// <summary>
            /// pack_and_cell_data.hall_sys_v.hall_v_supply
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_SYS_V_HALL_V_SUPPLY_DID = 963,

            /// <summary>
            /// pack_and_cell_data.hall_v.hall_v_supply
            /// </summary>
            PUDS_SVC_PARAM_DI_HALL_V_HALL_V_SUPPLY_DID = 964,

            /// <summary>
            /// pack_and_cell_data.cell_t.min.val
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MIN_VAL_DID = 965,

            /// <summary>
            /// pack_and_cell_data.cell_t.min.id.cmu
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MIN_ID_CMU_DID = 966,

            /// <summary>
            /// pack_and_cell_data.cell_t.min.id.cell
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MIN_ID_CELL_DID = 967,

            /// <summary>
            /// pack_and_cell_data.cell_t.max.val
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MAX_VAL_DID = 968,

            /// <summary>
            /// pack_and_cell_data.cell_t.max.id.cmu
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MAX_ID_CMU_DID = 969,

            /// <summary>
            /// pack_and_cell_data.cell_t.max.id.cell
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_MAX_ID_CELL_DID = 970,

            /// <summary>
            /// pack_and_cell_data.cell_t.avg
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_AVG_DID = 971,

            /// <summary>
            /// pack_and_cell_data.cell_t.num_available
            /// </summary>
            PUDS_SVC_PARAM_DI_CELL_T_NUM_AVAILABLE_DID = 972,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu1.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU1_CELL_BITMASK_DID = 973,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu2.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU2_CELL_BITMASK_DID = 974,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu3.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU3_CELL_BITMASK_DID = 975,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu4.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU4_CELL_BITMASK_DID = 976,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu5.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU5_CELL_BITMASK_DID = 977,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu6.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU6_CELL_BITMASK_DID = 978,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu7.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU7_CELL_BITMASK_DID = 979,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu8.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU8_CELL_BITMASK_DID = 980,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu9.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU9_CELL_BITMASK_DID = 981,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu10.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU10_CELL_BITMASK_DID = 982,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu11.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU11_CELL_BITMASK_DID = 983,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu12.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU12_CELL_BITMASK_DID = 984,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu13.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU13_CELL_BITMASK_DID = 985,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu14.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU14_CELL_BITMASK_DID = 986,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu15.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU15_CELL_BITMASK_DID = 987,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu16.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU16_CELL_BITMASK_DID = 988,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu17.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU17_CELL_BITMASK_DID = 989,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu18.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU18_CELL_BITMASK_DID = 990,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu19.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU19_CELL_BITMASK_DID = 991,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu20.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU20_CELL_BITMASK_DID = 992,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu21.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU21_CELL_BITMASK_DID = 993,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu22.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU22_CELL_BITMASK_DID = 994,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu23.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU23_CELL_BITMASK_DID = 995,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu24.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU24_CELL_BITMASK_DID = 996,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu25.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU25_CELL_BITMASK_DID = 997,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu26.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU26_CELL_BITMASK_DID = 998,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu27.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU27_CELL_BITMASK_DID = 999,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu28.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU28_CELL_BITMASK_DID = 1000,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu29.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU29_CELL_BITMASK_DID = 1001,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu30.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU30_CELL_BITMASK_DID = 1002,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu31.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU31_CELL_BITMASK_DID = 1003,

            /// <summary>
            /// state_data.balancing.balance_setting.cmu32.cell_bitmask
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_BALANCE_SETTING_CMU32_CELL_BITMASK_DID = 1004,

            /// <summary>
            /// cdp_storage.cdp_output1
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT1_DID = 1005,

            /// <summary>
            /// cdp_storage.cdp_output2
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT2_DID = 1006,

            /// <summary>
            /// cdp_storage.cdp_output3
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT3_DID = 1007,

            /// <summary>
            /// cdp_storage.cdp_output4
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT4_DID = 1008,

            /// <summary>
            /// cdp_storage.cdp_output5
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT5_DID = 1009,

            /// <summary>
            /// cdp_storage.cdp_output6
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT6_DID = 1010,

            /// <summary>
            /// cdp_storage.cdp_output7
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT7_DID = 1011,

            /// <summary>
            /// cdp_storage.cdp_output8
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT8_DID = 1012,

            /// <summary>
            /// cdp_storage.cdp_output9
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT9_DID = 1013,

            /// <summary>
            /// cdp_storage.cdp_output10
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT10_DID = 1014,

            /// <summary>
            /// cdp_storage.cdp_output11
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT11_DID = 1015,

            /// <summary>
            /// cdp_storage.cdp_output12
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT12_DID = 1016,

            /// <summary>
            /// cdp_storage.cdp_output13
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT13_DID = 1017,

            /// <summary>
            /// cdp_storage.cdp_output14
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT14_DID = 1018,

            /// <summary>
            /// cdp_storage.cdp_output15
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT15_DID = 1019,

            /// <summary>
            /// cdp_storage.cdp_output16
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT16_DID = 1020,

            /// <summary>
            /// cdp_storage.cdp_output17
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT17_DID = 1021,

            /// <summary>
            /// cdp_storage.cdp_output18
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT18_DID = 1022,

            /// <summary>
            /// cdp_storage.cdp_output19
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT19_DID = 1023,

            /// <summary>
            /// cdp_storage.cdp_output20
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT20_DID = 1024,

            /// <summary>
            /// cdp_storage.cdp_output21
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT21_DID = 1025,

            /// <summary>
            /// cdp_storage.cdp_output22
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT22_DID = 1026,

            /// <summary>
            /// cdp_storage.cdp_output23
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT23_DID = 1027,

            /// <summary>
            /// cdp_storage.cdp_output24
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT24_DID = 1028,

            /// <summary>
            /// cdp_storage.cdp_output25
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT25_DID = 1029,

            /// <summary>
            /// cdp_storage.cdp_output26
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT26_DID = 1030,

            /// <summary>
            /// cdp_storage.cdp_output27
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT27_DID = 1031,

            /// <summary>
            /// cdp_storage.cdp_output28
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT28_DID = 1032,
            /// <summary>
            /// cdp_storage.cdp_output29
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT29_DID = 1033,
            /// <summary>
            /// cdp_storage.cdp_output30
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT30_DID = 1034,
            /// <summary>
            /// cdp_storage.cdp_output31
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT31_DID = 1035,
            /// <summary>
            /// cdp_storage.cdp_output32
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT32_DID = 1036,
            /// <summary>
            /// cdp_storage.cdp_output33
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT33_DID = 1037,
            /// <summary>
            /// cdp_storage.cdp_output34
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT34_DID = 1038,
            /// <summary>
            /// cdp_storage.cdp_output35
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT35_DID = 1039,
            /// <summary>
            /// cdp_storage.cdp_output36
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT36_DID = 1040,
            /// <summary>
            /// cdp_storage.cdp_output37
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT37_DID = 1041,
            /// <summary>
            /// cdp_storage.cdp_output38
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT38_DID = 1042,
            /// <summary>
            /// cdp_storage.cdp_output39
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT39_DID = 1043,
            /// <summary>
            /// cdp_storage.cdp_output40
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT40_DID = 1044,
            /// <summary>
            /// cdp_storage.cdp_output41
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT41_DID = 1045,
            /// <summary>
            /// cdp_storage.cdp_output42
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT42_DID = 1046,
            /// <summary>
            /// cdp_storage.cdp_output43
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT43_DID = 1047,
            /// <summary>
            /// cdp_storage.cdp_output44
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT44_DID = 1048,
            /// <summary>
            /// cdp_storage.cdp_output45
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT45_DID = 1049,
            /// <summary>
            /// cdp_storage.cdp_output46
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT46_DID = 1050,
            /// <summary>
            /// cdp_storage.cdp_output47
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT47_DID = 1051,
            /// <summary>
            /// cdp_storage.cdp_output48
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT48_DID = 1052,
            /// <summary>
            /// cdp_storage.cdp_output49
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT49_DID = 1053,
            /// <summary>
            /// cdp_storage.cdp_output50
            /// </summary>
            PUDS_SVC_PARAM_DI_CDP_OUTPUT50_DID = 1054,
            /// <summary>
            /// psu_status.vdd5
            /// </summary>
            PUDS_SVC_PARAM_DI_VDD5_DID = 1072,

            /// <summary>
            /// psu_status.vdd6
            /// </summary>
            PUDS_SVC_PARAM_DI_VDD6_DID = 1073,

            /// <summary>
            /// psu_status.vsfb1
            /// </summary>
            PUDS_SVC_PARAM_DI_VSFB1_DID = 1074,

            /// <summary>
            /// psu_status.vbats
            /// </summary>
            PUDS_SVC_PARAM_DI_VBATS_DID = 1075,

            /// <summary>
            /// state_data.flags.waiting_for_staging_req
            /// </summary>
            PUDS_SVC_PARAM_DI_FLAGS_WAITING_FOR_STAGING_REQ_DID = 1076,

            /// <summary>
            /// sop_discharge[0]
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_DISCHARGE_0_DID = 1077,

            /// <summary>
            /// sop_discharge[1]
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_DISCHARGE_1_DID = 1078,

            /// <summary>
            /// sop_discharge[2]
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_DISCHARGE_2_DID = 1079,

            /// <summary>
            /// sop_discharge[3]
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_DISCHARGE_3_DID = 1080,

            /// <summary>
            /// soh
            /// </summary>
            PUDS_SVC_PARAM_DI_SOH_DID = 1081,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_V_REF2_DID = 1082,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_V_REF2_DID = 1083,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_V_REF2_DID = 1084,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_V_REF2_DID = 1085,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_V_REF2_DID = 1086,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_V_REF2_DID = 1087,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_V_REF2_DID = 1088,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_V_REF2_DID = 1089,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_V_REF2_DID = 1090,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_V_REF2_DID = 1091,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_V_REF2_DID = 1092,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_V_REF2_DID = 1093,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_V_REF2_DID = 1094,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_V_REF2_DID = 1095,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_V_REF2_DID = 1096,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_V_REF2_DID = 1097,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_V_REF2_DID = 1098,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_V_REF2_DID = 1099,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_V_REF2_DID = 1100,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_V_REF2_DID = 1101,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_V_REF2_DID = 1102,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_V_REF2_DID = 1103,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_V_REF2_DID = 1104,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_V_REF2_DID = 1105,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_V_REF2_DID = 1106,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_V_REF2_DID = 1107,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_V_REF2_DID = 1108,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_V_REF2_DID = 1109,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_V_REF2_DID = 1110,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_V_REF2_DID = 1111,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_V_REF2_DID = 1112,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.v_ref2
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_V_REF2_DID = 1113,

            /// <summary>
            /// config_id
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIG_ID_DID = 1114,

            /// <summary>
            /// config_crc
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIG_CRC_DID = 1115,

            /// <summary>
            /// dynamic_soc1
            /// </summary>
            PUDS_SVC_PARAM_DI_DYNAMIC_SOC1_DID = 1116,

            /// <summary>
            /// dynamic_soc2
            /// </summary>
            PUDS_SVC_PARAM_DI_DYNAMIC_SOC2_DID = 1117,

            /// <summary>
            /// dynamic_soc3
            /// </summary>
            PUDS_SVC_PARAM_DI_DYNAMIC_SOC3_DID = 1118,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire1
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE1_DID = 1119,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire2
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE2_DID = 1120,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire3
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE3_DID = 1121,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire4
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE4_DID = 1122,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire5
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE5_DID = 1123,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire6
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE6_DID = 1124,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire7
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE7_DID = 1125,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire8
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE8_DID = 1126,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire9
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE9_DID = 1127,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire10
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE10_DID = 1128,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire11
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE11_DID = 1129,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire12
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE12_DID = 1130,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire13
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE13_DID = 1131,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire14
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE14_DID = 1132,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire15
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE15_DID = 1133,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire16
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE16_DID = 1134,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire17
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE17_DID = 1135,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire18
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE18_DID = 1136,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire19
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE19_DID = 1137,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire20
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE20_DID = 1138,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire21
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE21_DID = 1139,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire22
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE22_DID = 1140,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire23
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE23_DID = 1141,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire24
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE24_DID = 1142,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire25
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE25_DID = 1143,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire26
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE26_DID = 1144,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire27
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE27_DID = 1145,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire28
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE28_DID = 1146,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire29
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE29_DID = 1147,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire30
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE30_DID = 1148,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire31
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE31_DID = 1149,

            /// <summary>
            /// pack_and_cell_data.safety_open_wire32
            /// </summary>
            PUDS_SVC_PARAM_DI_SAFETY_OPEN_WIRE32_DID = 1150,

            /// <summary>
            /// dcli_1
            /// </summary>
            PUDS_SVC_PARAM_DI_DCLI_1_DID = 1151,

            /// <summary>
            /// dclo_1
            /// </summary>
            PUDS_SVC_PARAM_DI_DCLO_1_DID = 1152,

            /// <summary>
            /// future_temperature
            /// </summary>
            PUDS_SVC_PARAM_DI_FUTURE_TEMPERATURE_DID = 1153,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V13_DID = 1200,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V14_DID = 1201,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V15_DID = 1202,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V16_DID = 1203,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V17_DID = 1204,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu1.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU1_CELL_V18_DID = 1205,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V13_DID = 1206,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V14_DID = 1207,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V15_DID = 1208,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V16_DID = 1209,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V17_DID = 1210,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu2.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU2_CELL_V18_DID = 1211,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V13_DID = 1212,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V14_DID = 1213,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V15_DID = 1214,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V16_DID = 1215,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V17_DID = 1216,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu3.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU3_CELL_V18_DID = 1217,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V13_DID = 1218,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V14_DID = 1219,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V15_DID = 1220,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V16_DID = 1221,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V17_DID = 1222,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu4.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU4_CELL_V18_DID = 1223,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V13_DID = 1224,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V14_DID = 1225,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V15_DID = 1226,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V16_DID = 1227,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V17_DID = 1228,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu5.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU5_CELL_V18_DID = 1229,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V13_DID = 1230,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V14_DID = 1231,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V15_DID = 1232,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V16_DID = 1233,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V17_DID = 1234,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu6.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU6_CELL_V18_DID = 1235,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V13_DID = 1236,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V14_DID = 1237,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V15_DID = 1238,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V16_DID = 1239,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V17_DID = 1240,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu7.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU7_CELL_V18_DID = 1241,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V13_DID = 1242,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V14_DID = 1243,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V15_DID = 1244,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V16_DID = 1245,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V17_DID = 1246,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu8.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU8_CELL_V18_DID = 1247,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V13_DID = 1248,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V14_DID = 1249,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V15_DID = 1250,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V16_DID = 1251,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V17_DID = 1252,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu9.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU9_CELL_V18_DID = 1253,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V13_DID = 1254,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V14_DID = 1255,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V15_DID = 1256,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V16_DID = 1257,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V17_DID = 1258,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu10.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU10_CELL_V18_DID = 1259,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V13_DID = 1260,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V14_DID = 1261,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V15_DID = 1262,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V16_DID = 1263,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V17_DID = 1264,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu11.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU11_CELL_V18_DID = 1265,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V13_DID = 1266,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V14_DID = 1267,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V15_DID = 1268,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V16_DID = 1269,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V17_DID = 1270,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu12.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU12_CELL_V18_DID = 1271,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V13_DID = 1272,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V14_DID = 1273,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V15_DID = 1274,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V16_DID = 1275,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V17_DID = 1276,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu13.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU13_CELL_V18_DID = 1277,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V13_DID = 1278,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V14_DID = 1279,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V15_DID = 1280,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V16_DID = 1281,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V17_DID = 1282,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu14.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU14_CELL_V18_DID = 1283,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V13_DID = 1284,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V14_DID = 1285,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V15_DID = 1286,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V16_DID = 1287,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V17_DID = 1288,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu15.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU15_CELL_V18_DID = 1289,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V13_DID = 1290,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V14_DID = 1291,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V15_DID = 1292,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V16_DID = 1293,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V17_DID = 1294,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu16.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU16_CELL_V18_DID = 1295,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V13_DID = 1296,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V14_DID = 1297,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V15_DID = 1298,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V16_DID = 1299,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V17_DID = 1300,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu17.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU17_CELL_V18_DID = 1301,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V13_DID = 1302,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V14_DID = 1303,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V15_DID = 1304,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V16_DID = 1305,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V17_DID = 1306,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu18.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU18_CELL_V18_DID = 1307,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V13_DID = 1308,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V14_DID = 1309,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V15_DID = 1310,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V16_DID = 1311,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V17_DID = 1312,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu19.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU19_CELL_V18_DID = 1313,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V13_DID = 1314,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V14_DID = 1315,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V15_DID = 1316,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V16_DID = 1317,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V17_DID = 1318,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu20.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU20_CELL_V18_DID = 1319,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V13_DID = 1320,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V14_DID = 1321,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V15_DID = 1322,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V16_DID = 1323,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V17_DID = 1324,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu21.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU21_CELL_V18_DID = 1325,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V13_DID = 1326,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V14_DID = 1327,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V15_DID = 1328,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V16_DID = 1329,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V17_DID = 1330,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu22.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU22_CELL_V18_DID = 1331,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V13_DID = 1332,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V14_DID = 1333,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V15_DID = 1334,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V16_DID = 1335,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V17_DID = 1336,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu23.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU23_CELL_V18_DID = 1337,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V13_DID = 1338,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V14_DID = 1339,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V15_DID = 1340,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V16_DID = 1341,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V17_DID = 1342,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu24.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU24_CELL_V18_DID = 1343,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V13_DID = 1344,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V14_DID = 1345,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V15_DID = 1346,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V16_DID = 1347,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V17_DID = 1348,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu25.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU25_CELL_V18_DID = 1349,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V13_DID = 1350,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V14_DID = 1351,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V15_DID = 1352,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V16_DID = 1353,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V17_DID = 1354,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu26.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU26_CELL_V18_DID = 1355,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V13_DID = 1356,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V14_DID = 1357,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V15_DID = 1358,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V16_DID = 1359,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V17_DID = 1360,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu27.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU27_CELL_V18_DID = 1361,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V13_DID = 1362,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V14_DID = 1363,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V15_DID = 1364,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V16_DID = 1365,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V17_DID = 1366,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu28.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU28_CELL_V18_DID = 1367,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V13_DID = 1368,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V14_DID = 1369,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V15_DID = 1370,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V16_DID = 1371,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V17_DID = 1372,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu29.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU29_CELL_V18_DID = 1373,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V13_DID = 1374,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V14_DID = 1375,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V15_DID = 1376,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V16_DID = 1377,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V17_DID = 1378,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu30.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU30_CELL_V18_DID = 1379,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V13_DID = 1380,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V14_DID = 1381,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V15_DID = 1382,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V16_DID = 1383,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V17_DID = 1384,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu31.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU31_CELL_V18_DID = 1385,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v13
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V13_DID = 1386,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v14
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V14_DID = 1387,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v15
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V15_DID = 1388,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v16
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V16_DID = 1389,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v17
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V17_DID = 1390,

            /// <summary>
            /// pack_and_cell_data.cmus.cmu32.cell_v18
            /// </summary>
            PUDS_SVC_PARAM_DI_CMUS_CMU32_CELL_V18_DID = 1391,


            PUDS_SVC_PARAM_DI_OP_DATA_TOTAL_UPTIME_DID = 1501,



            /// <summary>
            /// cmu_version
            /// </summary>
            PUDS_SVC_PARAM_DI_CMU_VERSION_DID = 3000,

            /// <summary>
            /// op_data.pack_resistance_u
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_RESISTANCE_U_DID = 3002,

            /// <summary>
            /// i_ma
            /// </summary>
            PUDS_SVC_PARAM_DI_I_MA_DID = 3003,

            /// <summary>
            /// i_charge_max
            /// </summary>
            PUDS_SVC_PARAM_DI_I_CHARGE_MAX_DID = 3004,

            /// <summary>
            /// chg_time_estimate
            /// </summary>
            PUDS_SVC_PARAM_DI_CHG_TIME_ESTIMATE_DID = 3005,

            /// <summary>
            /// charger_2_active
            /// </summary>
            PUDS_SVC_PARAM_DI_CHARGER_2_ACTIVE_DID = 3006,

            /// <summary>
            /// CONFIG.soc_ocv_data.initial_pack_resistance_100u
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIGSOC_OCV_DATA_INITIAL_PACK_RESISTANCE_100U_DID = 3007,

            /// <summary>
            /// soe_Ws
            /// </summary>
            PUDS_SVC_PARAM_DI_SOE_WS_DID = 3008,

            /// <summary>
            /// soe_full_Ws
            /// </summary>
            PUDS_SVC_PARAM_DI_SOE_FULL_WS_DID = 3009,

            /// <summary>
            /// rtc_wakeup_latched
            /// </summary>
            PUDS_SVC_PARAM_DI_RTC_WAKEUP_LATCHED_DID = 3010,

            /// <summary>
            /// uds_diagnostic_session
            /// </summary>
            PUDS_SVC_PARAM_DI_UDS_DIAGNOSTIC_SESSION_DID = 3011,

            /// <summary>
            /// CONFIG.lim_cell_v_min
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIGLIM_CELL_V_MIN_DID = 3012,

            /// <summary>
            /// CONFIG.lim_cell_v_max
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIGLIM_CELL_V_MAX_DID = 3013,

            /// <summary>
            /// CONFIG.config_version.type
            /// </summary>
            PUDS_SVC_PARAM_DI_CONFIGCONFIG_VERSION_TYPE_DID = 3014,

            /// <summary>
            /// balancing_performance
            /// </summary>
            PUDS_SVC_PARAM_DI_BALANCING_PERFORMANCE_DID = 3015,

            /// <summary>
            /// i2t_remain_charge
            /// </summary>
            PUDS_SVC_PARAM_DI_I2T_REMAIN_CHARGE_DID = 3016,

            /// <summary>
            /// sop_charge[0]
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_CHARGE_0_DID = 3017,

            /// <summary>
            /// sop_charge[1]
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_CHARGE_1_DID = 3018,

            /// <summary>
            /// sop_charge[2]
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_CHARGE_2_DID = 3019,

            /// <summary>
            /// sop_charge[3]
            /// </summary>
            PUDS_SVC_PARAM_DI_SOP_CHARGE_3_DID = 3020,

            /// <summary>
            /// r_pack1
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_RESISTANCE_MEASURED_DID = 3101,

            /// <summary>
            /// r_pack2
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_OCV_DID = 3102,

            /// <summary>
            /// raw_soc_ocv
            /// </summary>
            PUDS_SVC_PARAM_DI_RAW_SOC_OCV_DID = 3103,

            /// <summary>
            /// setup.shunt_r_100n
            /// </summary>
            PUDS_SVC_PARAM_DI_SHUNT_R_100N_DID = 3140,

            /// <summary>
            /// setup.shunt_offset
            /// </summary>
            PUDS_SVC_PARAM_DI_SHUNT_OFFSET_DID = 3141,

            /// <summary>
            /// setup.address
            /// </summary>
            PUDS_SVC_PARAM_DI_ADDRESS_DID = 3142,

            /// <summary>
            /// odo_data.P_total_charge_Ws
            /// </summary>
            PUDS_SVC_PARAM_DI_P_TOTAL_ENERGY_IN_WH_DID = 3143,

            /// <summary>
            /// odo_data.P_total_discharge_Ws
            /// </summary>
            PUDS_SVC_PARAM_DI_P_TOTAL_ENERGY_OUT_WH_DID = 3144,

            /// <summary>
            /// op_data.pack_cycle_number
            /// </summary>
            PUDS_SVC_PARAM_DI_PACK_CYCLE_NUMBER_DID = 3145,

            /// <summary>
            /// op_data.TOTAL_DISCHARGE_AH
            /// </summary>
            PUDS_SVC_PARAM_DI_TOTAL_DISCHARGE_AH_DID = 3146,

            /// <summary>
            /// op_data.TOTAL_DISCHARGE_AH
            /// </summary>
            PUDS_SVC_PARAM_DI_INTERNAL_SW_VERSION_DID = 3147,

            /// <summary>
            /// parallel_packs.is_master
            /// </summary>
            PUDS_SVC_PARAM_DI_IS_MASTER_DID = 4000,

            /// <summary>
            /// parallel_packs.unified_charger_output_current_100m
            /// </summary>
            PUDS_SVC_PARAM_DI_UNIFIED_REQUESTED_CHARGER_CURRENT_DID = 4001,

            /// <summary>
            /// parallel_packs.num_packs_active
            /// </summary>
            PUDS_SVC_PARAM_DI_NUM_PACKS_ACTIVE_DID = 4002,

            /// <summary>
            /// parallel_packs.unified_dcli
            /// </summary>
            PUDS_SVC_PARAM_DI_UNIFIED_DCLI_DID = 4003,

            /// <summary>
            /// parallel_packs.unified_dclo
            /// </summary>
            PUDS_SVC_PARAM_DI_UNIFIED_DCLO_DID = 4004,

            /// <summary>
            /// parallel_packs.unified_soc
            /// </summary>
            PUDS_SVC_PARAM_DI_UNIFIED_SOC_DID = 4005,

            /// <summary>
            /// parallel_packs.unified_voltage
            /// </summary>
            PUDS_SVC_PARAM_DI_NUM_PACKS_DID = 4006,

            /// <summary>
            /// parallel_packs.unified_current
            /// </summary>
            PUDS_SVC_PARAM_DI_UNIFIED_CURRENT_DID = 4007,

            /// <summary>
            /// parallel_packs.unified_full_charge
            /// </summary>
            PUDS_SVC_PARAM_DI_UNIFIED_FULL_CHARGE_DID = 4008,

            /// <summary>
            /// parallel_packs.unified_remaining_charge
            /// </summary>
            PUDS_SVC_PARAM_DI_UNIFIED_REMAINING_CHARGE_DID = 4009,

            /// <summary>
            /// parallel_packs.packs1.status
            /// </summary>
            PUDS_SVC_PARAM_DI_UNIFIED_REQUESTED_CHARGER_VOLTAGE_DID = 4010,

            /// <summary>
            /// parallel_packs.packs1.fet_closed
            /// </summary>
            PUDS_SVC_PARAM_DI_UNIFIED_SOH_DID = 4011,


            PUDS_SVC_PARAM_DI_SOX_HYBRID_SOC_CC_ERROR_100m_DID = 5233,
            PUDS_SVC_PARAM_DI_SOX_HYBRID_SOC_SOC_OCV_ERROR_100m = 5234,
            PUDS_SVC_PARAM_DI_SOX_HYBRID_SOC_CC_100m_DID = 5235,




            /// <summary>
            /// gpio_input.gpio_input1
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT1_DID = 20001,

            /// <summary>
            /// gpio_input.gpio_input2
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT2_DID = 20002,

            /// <summary>
            /// gpio_input.gpio_input3
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT3_DID = 20003,

            /// <summary>
            /// gpio_input.gpio_input4
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT4_DID = 20004,

            /// <summary>
            /// gpio_input.gpio_input5
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT5_DID = 20005,

            /// <summary>
            /// gpio_input.gpio_input6
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT6_DID = 20006,

            /// <summary>
            /// gpio_input.gpio_input7
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT7_DID = 20007,

            /// <summary>
            /// gpio_input.gpio_input8
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT8_DID = 20008,

            /// <summary>
            /// gpio_input.gpio_input9
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT9_DID = 20009,

            /// <summary>
            /// gpio_input.gpio_input10
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT10_DID = 20010,

            /// <summary>
            /// gpio_input.gpio_input11
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT11_DID = 20011,

            /// <summary>
            /// gpio_input.gpio_input12
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT12_DID = 20012,

            /// <summary>
            /// gpio_input.gpio_input13
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT13_DID = 20013,

            /// <summary>
            /// gpio_input.gpio_input14
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT14_DID = 20014,

            /// <summary>
            /// gpio_input.gpio_input15
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT15_DID = 20015,

            /// <summary>
            /// gpio_input.gpio_input16
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT16_DID = 20016,

            /// <summary>
            /// gpio_input.gpio_input_union
            /// </summary>
            PUDS_SVC_PARAM_DI_GPIO_INPUT_UNION_DID = 20017,

            /// <summary>
            /// aux_a.osc_slow
            /// </summary>
            PUDS_SVC_PARAM_DI_OSC_SLOW_DID = 30001,

            /// <summary>
            /// aux_a.osc_fast
            /// </summary>
            PUDS_SVC_PARAM_DI_OSC_FAST_DID = 30002,

            /// <summary>
            /// can_rx_storage.can_rx_data1
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA1_DID = 50001,

            /// <summary>
            /// can_rx_storage.can_rx_data2
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA2_DID = 50002,

            /// <summary>
            /// can_rx_storage.can_rx_data3
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA3_DID = 50003,

            /// <summary>
            /// can_rx_storage.can_rx_data4
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA4_DID = 50004,

            /// <summary>
            /// can_rx_storage.can_rx_data5
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA5_DID = 50005,

            /// <summary>
            /// can_rx_storage.can_rx_data6
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA6_DID = 50006,

            /// <summary>
            /// can_rx_storage.can_rx_data7
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA7_DID = 50007,

            /// <summary>
            /// can_rx_storage.can_rx_data8
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA8_DID = 50008,

            /// <summary>
            /// can_rx_storage.can_rx_data9
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA9_DID = 50009,

            /// <summary>
            /// can_rx_storage.can_rx_data10
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA10_DID = 50010,

            /// <summary>
            /// can_rx_storage.can_rx_data11
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA11_DID = 50011,

            /// <summary>
            /// can_rx_storage.can_rx_data12
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA12_DID = 50012,

            /// <summary>
            /// can_rx_storage.can_rx_data13
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA13_DID = 50013,

            /// <summary>
            /// can_rx_storage.can_rx_data14
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA14_DID = 50014,

            /// <summary>
            /// can_rx_storage.can_rx_data15
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA15_DID = 50015,

            /// <summary>
            /// can_rx_storage.can_rx_data16
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA16_DID = 50016,

            /// <summary>
            /// can_rx_storage.can_rx_data17
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA17_DID = 50017,

            /// <summary>
            /// can_rx_storage.can_rx_data18
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA18_DID = 50018,

            /// <summary>
            /// can_rx_storage.can_rx_data19
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA19_DID = 50019,

            /// <summary>
            /// can_rx_storage.can_rx_data20
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA20_DID = 50020,

            /// <summary>
            /// can_rx_storage.can_rx_data21
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA21_DID = 50021,

            /// <summary>
            /// can_rx_storage.can_rx_data22
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA22_DID = 50022,

            /// <summary>
            /// can_rx_storage.can_rx_data23
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA23_DID = 50023,

            /// <summary>
            /// can_rx_storage.can_rx_data24
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA24_DID = 50024,

            /// <summary>
            /// can_rx_storage.can_rx_data25
            /// </summary>
            PUDS_SVC_PARAM_DI_CAN_RX_DATA25_DID = 50025,



            /// <summary>
            /// first_error_log_entry
            /// </summary>
            PUDS_SVC_PARAM_DI_ERROR_LOG_FIRST_ENTRY = 65000,

            /// <summary>
            /// next_error_log_entry
            /// </summary>
            PUDS_SVC_PARAM_DI_ERROR_LOG_NEXT_ENTRY = 65001,

            /// <summary>
            /// first_active_error_entry
            /// </summary>
            PUDS_SVC_PARAM_DI_ACTIVE_ERROR_FIRST_ENTRY = 65002,

            /// <summary>
            /// next_active_error_entry
            /// </summary>
            PUDS_SVC_PARAM_DI_ACTIVE_ERROR_NEXT_ENTRY = 65003,

            /// <summary>
            /// hardware_serial_number
            /// </summary>
            PUDS_SVC_PARAM_DI_HARDWARE_SERIAL_NUMBER = 65004,

            PUDS_SVC_PARAM_DI_FW_VERSION = 65005,


            /// <summary>
            /// bootSoftwareIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_BSIDID = 0xF180,
            /// <summary>
            /// applicationSoftwareIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ASIDID = 0xF181,
            /// <summary>
            /// applicationDataIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ADIDID = 0xF182,
            /// <summary>
            /// bootSoftwareIdentificationDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_BSFPDID = 0xF183,
            /// <summary>
            /// applicationSoftwareFingerprintDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ASFPDID = 0xF184,
            /// <summary>
            /// applicationDataFingerprintDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ADFPDID = 0xF185,
            /// <summary>
            /// activeDiagnosticSessionDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ADSDID = 0xF186,
            /// <summary>
            /// vehicleManufacturerSparePartNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMSPNDID = 0xF187,
            /// <summary>
            /// vehicleManufacturerECUSoftwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMECUSNDID = 0xF188,
            /// <summary>
            /// vehicleManufacturerECUSoftwareVersionNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMECUSVNDID = 0xF189,
            /// <summary>
            /// systemSupplierIdentifierDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSIDDID = 0xF18A,
            /// <summary>
            /// ECUManufacturingDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ECUMDDID = 0xF18B,
            /// <summary>
            /// ECUSerialNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ECUSNDID = 0xF18C,
            /// <summary>
            /// supportedFunctionalUnitsDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SFUDID = 0xF18D,
            /// <summary>
            /// vehicleManufacturerKitAssemblyPartNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMKAPNDID = 0xF18E,
            /// <summary>
            /// VINDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VINDID = 0xF190,
            /// <summary>
            /// vehicleManufacturerECUHardwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_VMECUHNDID = 0xF191,
            /// <summary>
            /// systemSupplierECUHardwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUHWNDID = 0xF192,
            /// <summary>
            /// systemSupplierECUHardwareVersionNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUHWVNDID = 0xF193,
            /// <summary>
            /// systemSupplierECUSoftwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUSWNDID = 0xF194,
            /// <summary>
            /// systemSupplierECUSoftwareVersionNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SSECUSWVNDID = 0xF195,
            /// <summary>
            /// exhaustRegulationOrTypeApprovalNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_EROTANDID = 0xF196,
            /// <summary>
            /// systemNameOrEngineTypeDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_SNOETDID = 0xF197,
            /// <summary>
            /// repairShopCodeOrTesterSerialNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_RSCOTSNDID = 0xF198,
            /// <summary>
            /// programmingDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_PDDID = 0xF199,
            /// <summary>
            /// calibrationRepairShopCodeOrCalibrationEquipmentSerialNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_CRSCOCESNDID = 0xF19A,
            /// <summary>
            /// calibrationDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_CDDID = 0xF19B,
            /// <summary>
            /// calibrationEquipmentSoftwareNumberDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_CESWNDID = 0xF19C,
            /// <summary>
            /// ECUInstallationDateDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_EIDDID = 0xF19D,
            /// <summary>
            /// ODXFileDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_ODXFDID = 0xF19E,
            /// <summary>
            /// entityDataIdentifier
            /// </summary>
            PUDS_SVC_PARAM_DI_EDID = 0xF19F,
        }


        /// <summary>
        /// Data Identifiers ISO-14229-1:2006 §C.1 p.259
        /// </summary>
        public enum TPUDSSvcParamDIN3ParallelPack : ushort
        {
            PARALLEL_PACKS_INITIALLY_DETECTED_PACKS = 48325,

            //Pack current : Data type: float64
            PARALLEL_PACKS_PPAID1_PACK_CURRENT = 48328,
            PARALLEL_PACKS_PPAID2_PACK_CURRENT = 48329,
            PARALLEL_PACKS_PPAID3_PACK_CURRENT = 48330,
            PARALLEL_PACKS_PPAID4_PACK_CURRENT = 48331,
            PARALLEL_PACKS_PPAID5_PACK_CURRENT = 48332,
            PARALLEL_PACKS_PPAID6_PACK_CURRENT = 48333,
            PARALLEL_PACKS_PPAID7_PACK_CURRENT = 48334,
            PARALLEL_PACKS_PPAID8_PACK_CURRENT = 48335,
            PARALLEL_PACKS_PPAID9_PACK_CURRENT = 48336,
            PARALLEL_PACKS_PPAID10_PACK_CURRENT = 48337,

            PARALLEL_PACKS_AGGREGATED_MIN_CELL_TEMPERATURE = 48532, //Uint16
            PARALLEL_PACKS_AGGREGATED_MAX_CELL_TEMPERATURE = 48533,

            PARALLEL_PACKS_AGGREGATED_SOC_TOTAL = 48519,
            PARALLEL_PACKS_AGGREGATED_SOH_TOTAL = 48520,

            PARALLEL_PACKS_AGGREGATED_DCLI = 48528,
            PARALLEL_PACKS_AGGREGATED_DCLO = 48529,

            PARALLEL_PACKS_AGGREGATED_MIN_CELL_VOLTAGE = 48534,//Uint16
            PARALLEL_PACKS_AGGREGATED_MAX_CELL_VOLTAGE = 48535,

            PARALLEL_PACKS_AGGREGATED_SYSTEM_STATE = 48537,//Uint8
            PARALLEL_PACKS_AGGREGATED_CHARGE_COMPLETE_STATUS = 48536,

            PARALLEL_PACKS_AGGREGATED_BALANCING_STATUS = 48530,
            PARALLEL_PACKS_AGGREGATED_CONTACTOR_WELD_STATUS = 48531,
            PARALLEL_PACKS_AGGREGATED_CHARGE_CURRENT = 48523,

            PARALLEL_PACKS_PPAID1_MAX_CELL_VOLTAGE = 48348,
            PARALLEL_PACKS_PPAID2_MAX_CELL_VOLTAGE = 48349,
            PARALLEL_PACKS_PPAID3_MAX_CELL_VOLTAGE = 48350,
            PARALLEL_PACKS_PPAID4_MAX_CELL_VOLTAGE = 48351,
            PARALLEL_PACKS_PPAID5_MAX_CELL_VOLTAGE = 48352,
            PARALLEL_PACKS_PPAID6_MAX_CELL_VOLTAGE = 48353,
            PARALLEL_PACKS_PPAID7_MAX_CELL_VOLTAGE = 48354,
            PARALLEL_PACKS_PPAID8_MAX_CELL_VOLTAGE = 48355,
            PARALLEL_PACKS_PPAID9_MAX_CELL_VOLTAGE = 48356,
            PARALLEL_PACKS_PPAID10_MAX_CELL_VOLTAGE = 48357,

            PARALLEL_PACKS_PPAID1_MIN_CELL_VOLTAGE = 48358,
            PARALLEL_PACKS_PPAID2_MIN_CELL_VOLTAGE = 48359,
            PARALLEL_PACKS_PPAID3_MIN_CELL_VOLTAGE = 48360,
            PARALLEL_PACKS_PPAID4_MIN_CELL_VOLTAGE = 48361,
            PARALLEL_PACKS_PPAID5_MIN_CELL_VOLTAGE = 48362,
            PARALLEL_PACKS_PPAID6_MIN_CELL_VOLTAGE = 48363,
            PARALLEL_PACKS_PPAID7_MIN_CELL_VOLTAGE = 48364,
            PARALLEL_PACKS_PPAID8_MIN_CELL_VOLTAGE = 48365,
            PARALLEL_PACKS_PPAID9_MIN_CELL_VOLTAGE = 48366,
            PARALLEL_PACKS_PPAID10_MIN_CELL_VOLTAGE = 48367,

            PARALLEL_PACKS_PPAID1_PACK_VOLTAGE = 48338,
            PARALLEL_PACKS_PPAID2_PACK_VOLTAGE = 48339,
            PARALLEL_PACKS_PPAID3_PACK_VOLTAGE = 48340,
            PARALLEL_PACKS_PPAID4_PACK_VOLTAGE = 48341,
            PARALLEL_PACKS_PPAID5_PACK_VOLTAGE = 48342,
            PARALLEL_PACKS_PPAID6_PACK_VOLTAGE = 48343,
            PARALLEL_PACKS_PPAID7_PACK_VOLTAGE = 48344,
            PARALLEL_PACKS_PPAID8_PACK_VOLTAGE = 48345,
            PARALLEL_PACKS_PPAID9_PACK_VOLTAGE = 48346,
            PARALLEL_PACKS_PPAID10_PACK_VOLTAGE = 48347,
        }
        /// <summary>
        ///	The ReadDataByIdentifier service allows the client to request data record values 
        ///	from the server identified by one or more dataIdentifiers.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="Buffer">buffer containing a list of two-byte Data Identifiers (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="BufferLength">Number of elements in the buffer (size in WORD of the buffer)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDataByIdentifier")]
        public static extern TPUDSStatus SvcReadDataByIdentifier(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            ushort[] Buffer,
            ushort BufferLength);
        #endregion

        #region UDS Service: ReadMemoryByAddress
        // ISO-15765-3:2004 §9.3.2 p.47 && ISO-14229-1:2006 §10.3 p.102

        /// <summary>
        ///	The ReadMemoryByAddress service allows the client to request memory data from the server 
        ///	via a provided starting address and to specify the size of memory to be read.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="MemoryAddress">starting address of server memory from which data is to be retrieved</param>
        /// <param name="MemoryAddressLength">Size in bytes of the MemoryAddress buffer (max.: 0xF)</param>
        /// <param name="MemorySize">number of bytes to be read starting at the address specified by memoryAddress</param>
        /// <param name="MemorySizeLength">Size in bytes of the MemorySize buffer (max.: 0xF)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadMemoryByAddress")]
        public static extern TPUDSStatus SvcReadMemoryByAddress(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            byte[] MemoryAddress,
            byte MemoryAddressLength,
            byte[] MemorySize,
            byte MemorySizeLength);
        #endregion

        #region UDS Service: ReadScalingDataByIdentifier
        // ISO-15765-3:2004 §9.3.3 p.48 && ISO-14229-1:2006 §10.4 p.106

        /// <summary>
        ///	The ReadScalingDataByIdentifier service allows the client to request 
        ///	scaling data record information from the server identified by a dataIdentifier.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="DataIdentifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadScalingDataByIdentifier")]
        public static extern TPUDSStatus SvcReadScalingDataByIdentifier(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            ushort DataIdentifier);
        #endregion

        #region UDS Service: ReadDataByPeriodicIdentifier
        // ISO-15765-3:2004 §9.3.4 p.48 && ISO-14229-1:2006 §10.5 p.112

        /// <summary>
        /// TransmissionMode: Subfunction parameter for UDS service ReadDataByPeriodicIdentifier
        /// </summary>
        public enum TPUDSSvcParamRDBPI : byte
        {
            /// <summary>
            /// Send At Slow Rate
            /// </summary>
            PUDS_SVC_PARAM_RDBPI_SASR = 0x01,
            /// <summary>
            /// Send At Medium Rate
            /// </summary>
            PUDS_SVC_PARAM_RDBPI_SAMR = 0x02,
            /// <summary>
            /// Send At Fast Rate
            /// </summary>
            PUDS_SVC_PARAM_RDBPI_SAFR = 0x03,
            /// <summary>
            /// Stop Sending
            /// </summary>
            PUDS_SVC_PARAM_RDBPI_SS = 0x04,
        }

        /// <summary>
        ///	The ReadDataByPeriodicIdentifier service allows the client to request the periodic transmission 
        ///	of data record values from the server identified by one or more periodicDataIdentifiers.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="TransmissionMode">transmission rate code (see PUDS_SVC_PARAM_RDBPI_xxx)</param>
        /// <param name="Buffer">buffer containing a list of Periodic Data Identifiers</param>
        /// <param name="BufferLength">Number of elements in the buffer (size in WORD of the buffer)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDataByPeriodicIdentifier")]
        public static extern TPUDSStatus SvcReadDataByPeriodicIdentifier(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamRDBPI TransmissionMode,
            byte[] Buffer,
            ushort BufferLength);
        #endregion

        #region UDS Service: DynamicallyDefineDataIdentifier
        // ISO-15765-3:2004 §9.3.5 p.54 && ISO-14229-1:2006 §10.6 p.123

        /// <summary>
        /// DynamicallyDefineDataIdentifier Type: Subfunction parameter for UDS service DynamicallyDefineDataIdentifier
        /// </summary>
        public enum TPUDSSvcParamDDDI : byte
        {
            /// <summary>
            /// Define By Identifier
            /// </summary>
            PUDS_SVC_PARAM_DDDI_DBID = 0x01,
            /// <summary>
            /// Define By Memory Address
            /// </summary>
            PUDS_SVC_PARAM_DDDI_DBMA = 0x02,
            /// <summary>
            /// Clear Dynamically Defined Data Identifier
            /// </summary>
            PUDS_SVC_PARAM_DDDI_CDDDI = 0x03,
        }

        /// <summary>
        ///	The DynamicallyDefineDataIdentifier service allows the client to dynamically define 
        ///	in a server a data identifier that can be read via the ReadDataByIdentifier service at a later time.
        ///	The Define By Identifier subfunction specifies that definition of the dynamic data
        ///	identifier shall occur via a data identifier reference.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="DynamicallyDefinedDataIdentifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="SourceDataIdentifier">buffer containing the sources of information to be included into the dynamic data record</param>
        /// <param name="MemorySize">buffer containing the total numbers of bytes from the source data record address</param>
        /// <param name="PositionInSourceDataRecord">buffer containing the starting byte positions of the excerpt of the source data record</param>
        /// <param name="BuffersLength">Number of elements in the buffers (SourceDataIdentifier, MemoryAddress and PositionInSourceDataRecord)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcDynamicallyDefineDataIdentifierDBID")]
        public static extern TPUDSStatus SvcDynamicallyDefineDataIdentifierDBID(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            ushort DynamicallyDefinedDataIdentifier,
            ushort[] SourceDataIdentifier,
            byte[] MemorySize,
            byte[] PositionInSourceDataRecord,
            ushort BuffersLength);

        /// <summary>
        ///	The DynamicallyDefineDataIdentifier service allows the client to dynamically define 
        ///	in a server a data identifier that can be read via the ReadDataByIdentifier service at a later time.
        ///	The Define By Memory Address subfunction specifies that definition of the dynamic data
        ///	identifier shall occur via an address reference.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="DynamicallyDefinedDataIdentifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="MemoryAddressLength">Size in bytes of the MemoryAddress items in the MemoryAddressBuffer buffer (max.: 0xF)</param>
        /// <param name="MemorySizeLength">Size in bytes of the MemorySize items in the MemorySizeBuffer buffer (max.: 0xF)</param>
        /// <param name="MemoryAddressBuffer">buffer containing the MemoryAddress buffers,
        ///	must be an array of 'BuffersLength' entries which contains 'MemoryAddressLength' bytes
        ///	(size is 'BuffersLength * MemoryAddressLength' bytes)</param>
        /// <param name="MemorySizeBuffer">buffer containing the MemorySize buffers,
        ///	must be an array of 'BuffersLength' entries which contains 'MemorySizeLength' bytes
        ///	(size is 'BuffersLength * MemorySizeLength' bytes)</param>
        /// <param name="BuffersLength">Size in bytes of the MemoryAddressBuffer and MemorySizeBuffer buffers</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcDynamicallyDefineDataIdentifierDBMA")]
        public static extern TPUDSStatus SvcDynamicallyDefineDataIdentifierDBMA(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            ushort DynamicallyDefinedDataIdentifier,
            byte MemoryAddressLength,
            byte MemorySizeLength,
            byte[] MemoryAddressBuffer,
            byte[] MemorySizeBuffer,
            ushort BuffersLength);

        /// <summary>
        ///	The Clear Dynamically Defined Data Identifier subfunction shall be used to clear 
        ///	the specified dynamic data identifier.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="DynamicallyDefinedDataIdentifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcDynamicallyDefineDataIdentifierCDDDI")]
        public static extern TPUDSStatus SvcDynamicallyDefineDataIdentifierCDDDI(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            ushort DynamicallyDefinedDataIdentifier);
        #endregion

        #region UDS Service: WriteDataByIdentifier
        // ISO-15765-3:2004 §9.3.6 p.54 && ISO-14229-1:2006 §10.7 p.143

        /// <summary>
        ///	The WriteDataByIdentifier service allows the client to write information into the server at an internal location
        ///	specified by the provided data identifier.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="DataIdentifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="Buffer">buffer containing the data to write</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcWriteDataByIdentifier")]
        public static extern TPUDSStatus SvcWriteDataByIdentifier(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            ushort DataIdentifier,
            byte[] Buffer,
            ushort BufferLength);
        #endregion

        #region UDS Service: WriteMemoryByAddress
        // ISO-15765-3:2004 §9.3.7 p.54 && ISO-14229-1:2006 §10.8 p.146

        /// <summary>
        ///	The WriteMemoryByAddress service allows the client to write 
        ///	information into the server at one or more contiguous memory locations.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="MemoryAddress">starting address of server memory to which data is to be written</param>
        /// <param name="MemoryAddressLength">Size in bytes of the MemoryAddress buffer (max.: 0xF)</param>
        /// <param name="MemorySize">number of bytes to be written starting at the address specified by memoryAddress</param>
        /// <param name="MemorySizeLength">Size in bytes of the MemorySize buffer (max.: 0xF)</param>
        /// <param name="Buffer">buffer containing the data to write</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcWriteMemoryByAddress")]
        public static extern TPUDSStatus SvcWriteMemoryByAddress(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            byte[] MemoryAddress,
            byte MemoryAddressLength,
            byte[] MemorySize,
            byte MemorySizeLength,
            byte[] Buffer,
            ushort BufferLength);
        #endregion

        #region UDS Service: ClearDiagnosticInformation
        // ISO-15765-3:2004 §9.4.2 p.56 && ISO-14229-1:2006 §11.2 p.152

        /// <summary>
        /// groupOfDTC : Emissions-related systems group of DTCs
        /// </summary>  
        public const UInt32 PUDS_SVC_PARAM_CDI_ERS = 0x000000;
        /// <summary>
        /// groupOfDTC : All Groups of DTCs
        /// </summary>  
        public const UInt32 PUDS_SVC_PARAM_CDI_AGDTC = 0xFFFFFF;

        /// <summary>
        ///	The ClearDiagnosticInformation service is used by the client to clear diagnostic information 
        ///	in one server's or multiple servers’ memory.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="groupOfDTC">a three-byte value indicating the group of DTCs (e.g. powertrain, body, chassis) 
        /// or the particular DTC to be cleared (see PUDS_SVC_PARAM_CDI_xxx)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcClearDiagnosticInformation")]
        public static extern TPUDSStatus SvcClearDiagnosticInformation(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            UInt32 groupOfDTC);
        #endregion

        #region UDS Service: ReadDTCInformation
        // ISO-15765-3:2004 §9.4.1 p.54 && ISO-14229-1:2006 §11.3 p.154

        /// <summary>
        /// RDTCIType: Subfunction parameter for UDS service ReadDTCInformation
        /// ISO-15765-3:2004 §9.4.1 p.54 && ISO-14229-1:2006 §11.3 p.154
        /// </summary>
        public enum TPUDSSvcParamRDTCI : byte
        {
            /// <summary>
            /// report Number Of DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RNODTCBSM = 0x01,
            /// <summary>
            /// report DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCBSM = 0x02,
            /// <summary>
            /// report DTC Snapshot Identification
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCSSI = 0x03,
            /// <summary>
            /// report DTC Snapshot Record By DTC Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCSSBDTC = 0x04,
            /// <summary>
            /// report DTC Snapshot Record By Record Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCSSBRN = 0x05,
            /// <summary>
            /// report DTC Extended Data Record By DTC Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCEDRBDN = 0x06,
            /// <summary>
            /// report Number Of DTC By Severity Mask Record
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RNODTCBSMR = 0x07,
            /// <summary>
            /// report DTC By Severity Mask Record
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCBSMR = 0x08,
            /// <summary>
            /// report Severity Information Of DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RSIODTC = 0x09,
            /// <summary>
            /// report Supported DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RSUPDTC = 0x0A,
            /// <summary>
            /// report First Test Failed DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RFTFDTC = 0x0B,
            /// <summary>
            /// report First Confirmed DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RFCDTC = 0x0C,
            /// <summary>
            /// report Most Recent Test Failed DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RMRTFDTC = 0x0D,
            /// <summary>
            /// report Most Recent Confirmed DTC
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RMRCDTC = 0x0E,
            /// <summary>
            /// report Mirror Memory DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RMMDTCBSM = 0x0F,
            /// <summary>
            /// report Mirror Memory DTC Extended Data Record By DTC Number
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RMMDEDRBDN = 0x10,
            /// <summary>
            /// report Number Of Mirror MemoryDTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RNOMMDTCBSM = 0x11,
            /// <summary>
            /// report Number Of Emissions Related OBD DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RNOOBDDTCBSM = 0x12,
            /// <summary>
            /// report Emissions Related OBD DTC By Status Mask
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_ROBDDTCBSM = 0x13,
            /// <summary>
            /// report DTC Fault Detection Counter 
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCFDC = 0x14,
            /// <summary>
            /// report DTC With Permanent Status
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_RDTCWPS = 0x15,
        }

        /// <summary>
        /// DTCSeverityMask (DTCSVM) : ISO-14229-1:2006 §D.3 p.285
        /// </summary>
        [Flags]
        public enum TPUDSSvcParamRDTCI_DTCSVM : byte
        {
            /// <summary>
            /// DTC severity bit definitions : no SeverityAvailable
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_DTCSVM_NSA = 0x00,
            /// <summary>
            /// DTC severity bit definitions : maintenance Only
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_DTCSVM_MO = 0x20,
            /// <summary>
            /// DTC severity bit definitions : check At Next Halt
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_DTCSVM_CHKANH = 0x40,
            /// <summary>
            /// DTC severity bit definitions : check Immediately
            /// </summary>
            PUDS_SVC_PARAM_RDTCI_DTCSVM_CHKI = 0x80,
        }

        /// <summary>
        ///	This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// Only reportNumberOfDTCByStatusMask, reportDTCByStatusMask, reportMirrorMemoryDTCByStatusMask,
        ///	reportNumberOfMirrorMemoryDTCByStatusMask, reportNumberOfEmissionsRelatedOBDDTCByStatusMask, 
        ///	reportEmissionsRelatedOBDDTCByStatusMask Sub-functions are allowed.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="RDTCIType">Subfunction parameter: ReadDTCInformation type, use one of the following:
        ///	PUDS_SVC_PARAM_RDTCI_RNODTCBSM, PUDS_SVC_PARAM_RDTCI_RDTCBSM,
        ///	PUDS_SVC_PARAM_RDTCI_RMMDTCBSM, PUDS_SVC_PARAM_RDTCI_RNOMMDTCBSM,
        ///	PUDS_SVC_PARAM_RDTCI_RNOOBDDTCBSM, PUDS_SVC_PARAM_RDTCI_ROBDDTCBSM</param>
        /// <param name="DTCStatusMask">Contains eight DTC status bit.</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformation")]
        public static extern TPUDSStatus SvcReadDTCInformation(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamRDTCI RDTCIType,
            byte DTCStatusMask);

        /// <summary>
        ///	This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        ///	The sub-function reportDTCSnapshotRecordByDTCNumber (PUDS_SVC_PARAM_RDTCI_RDTCSSBDTC) is implicit.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="DTCMask">a unique identification number (three byte value) for a specific diagnostic trouble code</param>
        /// <param name="DTCSnapshotRecordNumber">the number of the specific DTCSnapshot data records</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRDTCSSBDTC")]
        public static extern TPUDSStatus SvcReadDTCInformationRDTCSSBDTC(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            UInt32 DTCMask,
            byte DTCSnapshotRecordNumber);

        /// <summary>
        ///	This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        ///	The sub-function reportDTCSnapshotByRecordNumber (PUDS_SVC_PARAM_RDTCI_RDTCSSBRN) is implicit.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="DTCSnapshotRecordNumber">the number of the specific DTCSnapshot data records</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRDTCSSBRN")]
        public static extern TPUDSStatus SvcReadDTCInformationRDTCSSBRN(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            byte DTCSnapshotRecordNumber);

        /// <summary>
        ///	This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// Only reportDTCExtendedDataRecordByDTCNumber and reportMirrorMemoryDTCExtendedDataRecordByDTCNumber Sub-functions are allowed.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="RDTCIType">Subfunction parameter: ReadDTCInformation type, use one of the following:
        ///	PUDS_SVC_PARAM_RDTCI_RDTCEDRBDN, PUDS_SVC_PARAM_RDTCI_RMMDEDRBDN</param>
        /// <param name="DTCMask">a unique identification number (three byte value) for a specific diagnostic trouble code</param>
        /// <param name="DTCExtendedDataRecordNumber">the number of the specific DTCExtendedData record requested.</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationReportExtended")]
        public static extern TPUDSStatus SvcReadDTCInformationReportExtended(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamRDTCI RDTCIType,
            UInt32 DTCMask,
            byte DTCExtendedDataRecordNumber);

        /// <summary>
        ///	This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// Only reportNumberOfDTCBySeverityMaskRecord and reportDTCSeverityInformation Sub-functions are allowed.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="RDTCIType">Subfunction parameter: ReadDTCInformation type, use one of the following:
        ///	PUDS_SVC_PARAM_RDTCI_RNODTCBSMR, PUDS_SVC_PARAM_RDTCI_RDTCBSMR</param>
        /// <param name="DTCSeverityMask">a mask of eight (8) DTC severity bits (see PUDS_SVC_PARAM_RDTCI_DTCSVM_xxx)</param>
        /// <param name="DTCStatusMask">a mask of eight (8) DTC status bits</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationReportSeverity")]
        public static extern TPUDSStatus SvcReadDTCInformationReportSeverity(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamRDTCI RDTCIType,
            byte DTCSeverityMask,
            byte DTCStatusMask);

        /// <summary>
        ///	This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        ///	The sub-function reportSeverityInformationOfDTC (PUDS_SVC_PARAM_RDTCI_RSIODTC) is implicit.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="DTCMask">a unique identification number for a specific diagnostic trouble code</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationRSIODTC")]
        public static extern TPUDSStatus SvcReadDTCInformationRSIODTC(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            UInt32 DTCMask);

        /// <summary>
        ///	This service allows a client to read the status of server-resident Diagnostic Trouble Code (DTC) information.
        /// Only reportSupportedDTC, reportFirstTestFailedDTC, reportFirstConfirmedDTC, reportMostRecentTestFailedDTC,
        ///	reportMostRecentConfirmedDTC, reportDTCFaultDetectionCounter, reportDTCWithPermanentStatus, 
        /// and reportDTCSnapshotIdentification Sub-functions are allowed.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="RDTCIType">Subfunction parameter: ReadDTCInformation type, use one of the following:
        ///	PUDS_SVC_PARAM_RDTCI_RFTFDTC, PUDS_SVC_PARAM_RDTCI_RFCDTC, 
        ///	PUDS_SVC_PARAM_RDTCI_RMRTFDTC, PUDS_SVC_PARAM_RDTCI_RMRCDTC, 
        ///	PUDS_SVC_PARAM_RDTCI_RSUPDTC, PUDS_SVC_PARAM_RDTCI_RDTCWPS,
        ///	PUDS_SVC_PARAM_RDTCI_RDTCSSI</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcReadDTCInformationNoParam")]
        public static extern TPUDSStatus SvcReadDTCInformationNoParam(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamRDTCI RDTCIType);
        #endregion

        #region UDS Service: InputOutputControlByIdentifier
        // ISO-15765-3:2004 §9.5.1 p.56 && ISO-14229-1:2006 §12.2 p.209, 

        /// <summary>
        /// inputOutputControlParameter: ISO-14229-1:2006  §E.1 p.289
        /// </summary>
        public enum TPUDSSvcParamIOCBI : byte
        {
            /// <summary>
            /// returnControlToECU (0 controlState bytes in request)
            /// </summary>
            PUDS_SVC_PARAM_IOCBI_RCTECU = 0x00,
            /// <summary>
            /// resetToDefault (0 controlState bytes in request)
            /// </summary>
            PUDS_SVC_PARAM_IOCBI_RTD = 0x01,
            /// <summary>
            /// freezeCurrentState (0 controlState bytes in request)
            /// </summary>
            PUDS_SVC_PARAM_IOCBI_FCS = 0x02,
            /// <summary>
            /// shortTermAdjustment
            /// </summary>
            PUDS_SVC_PARAM_IOCBI_STA = 0x03,
        }

        /// <summary>
        ///	The InputOutputControlByIdentifier service is used by the client to substitute a value for an input signal,
        ///	internal server function and/or control an output (actuator) of an electronic system.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="DataIdentifier">a two-byte Data Identifier (see PUDS_SVC_PARAM_DI_xxx)</param>
        /// <param name="ControlOptionRecord">First byte can be used as either an InputOutputControlParameter 
        ///	that describes how the server shall control its inputs or outputs (see PUDS_SVC_PARAM_IOCBI_xxx),
        ///	or as an additional controlState byte</param>
        /// <param name="ControlOptionRecordLength">Size in bytes of the ControlOptionRecord buffer</param>
        /// <param name="ControlEnableMaskRecord">The ControlEnableMask shall only be supported when 
        ///	the inputOutputControlParameter is used and the dataIdentifier to be controlled consists 
        ///	of more than one parameter (i.e. the dataIdentifier is bit-mapped or packeted by definition). 
        ///	There shall be one bit in the ControlEnableMask corresponding to each individual parameter 
        ///	defined within the dataIdentifier.</param>
        /// <param name="ControlEnableMaskRecordLength">Size in bytes of the controlEnableMaskRecord buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcInputOutputControlByIdentifier")]
        public static extern TPUDSStatus SvcInputOutputControlByIdentifier(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            ushort DataIdentifier,
            byte[] ControlOptionRecord,
            ushort ControlOptionRecordLength,
            byte[] ControlEnableMaskRecord,
            ushort ControlEnableMaskRecordLength);
        #endregion

        #region UDS Service: RoutineControl
        // ISO-15765-3:2004 §9.6.1 p.56 && ISO-14229-1:2006 §13.2 p.225

        /// <summary>
        /// RoutineControlType: Subfunction parameter for UDS service RoutineControl
        /// </summary>
        public enum TPUDSSvcParamRC : byte
        {
            /// <summary>
            /// Start Routine
            /// </summary>
            PUDS_SVC_PARAM_RC_STR = 0x01,
            /// <summary>
            /// Stop Routine
            /// </summary>
            PUDS_SVC_PARAM_RC_STPR = 0x02,
            /// <summary>
            /// Request Routine Results
            /// </summary>
            PUDS_SVC_PARAM_RC_RRR = 0x03,
        }

        /// <summary>
        /// Routine Identifier: ISO-14229-1:2006 §F.1 p.290
        /// </summary>
        public enum TPUDSSvcParamRC_RID : ushort
        {
            /// <summary>
            /// Deploy Loop Routine ID
            /// </summary>
            PUDS_SVC_PARAM_RC_RID_DLRI_ = 0xE200,
            /// <summary>
            /// erase Memory
            /// </summary>
            PUDS_SVC_PARAM_RC_RID_EM_ = 0xFF00,
            /// <summary>
            /// check Programming Dependencies
            /// </summary>
            PUDS_SVC_PARAM_RC_RID_CPD_ = 0xFF01,
            /// <summary>
            /// erase Mirror Memory DTCs
            /// </summary>
            PUDS_SVC_PARAM_RC_RID_EMMDTC_ = 0xFF02,
        }

        /// <summary>
        ///	The RoutineControl service is used by the client to start/stop a routine,
        ///	and request routine results.
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message</param>
        /// <param name="RoutineControlType">Subfunction parameter: RoutineControl type (see PUDS_SVC_PARAM_RC_xxx)</param>
        /// <param name="RoutineIdentifier">Server Local Routine Identifier (see PUDS_SVC_PARAM_RC_RID_xxx)</param>
        /// <param name="Buffer">buffer containing the Routine Control Options (only with start and stop routine sub-functions)</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRoutineControl")]
        public static extern TPUDSStatus SvcRoutineControl(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            TPUDSSvcParamRC RoutineControlType,
            ushort RoutineIdentifier,
            byte[] Buffer,
            ushort BufferLength);
        #endregion

        #region UDS Service: requestDownload
        // ISO-15765-3:2004 §9.7.1 p.57 && ISO-14229-1:2006 §14.2 p.231

        /// <summary>
        ///	The requestDownload service is used by the client to initiate a data transfer 
        ///	from the client to the server (download).
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="CompressionMethod">A nibble-value that specifies the "compressionMethod",	
        ///	The value 0x0 specifies that no compressionMethod is used.</param>
        /// <param name="EncryptingMethod">A nibble-value that specifies the "encryptingMethod",
        ///	The value 0x0 specifies that no encryptingMethod is used.</param>
        /// <param name="MemoryAddress">starting address of server memory to which data is to be written</param>
        /// <param name="MemoryAddressLength">Size in bytes of the MemoryAddress buffer (max.: 0xF)</param>
        /// <param name="MemorySize">used by the server to compare the uncompressed memory size with 
        ///	the total amount of data transferred during the TransferData service</param>
        /// <param name="MemorySizeLength">Size in bytes of the MemorySize buffer (max.: 0xF)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestDownload")]
        public static extern TPUDSStatus SvcRequestDownload(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            byte CompressionMethod,
            byte EncryptingMethod,
            byte[] MemoryAddress,
            byte MemoryAddressLength,
            byte[] MemorySize,
            byte MemorySizeLength);
        #endregion

        #region UDS Service: requestUpload
        // ISO-15765-3:2004 §9.7.1 p.57 && ISO-14229-1:2006 §14.3 p.234

        /// <summary>
        ///	The requestUpload service is used by the client to initiate a data transfer 
        ///	from the server to the client (upload).
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="CompressionMethod">A nibble-value that specifies the "compressionMethod",	
        ///	The value 0x0 specifies that no compressionMethod is used.</param>
        /// <param name="EncryptingMethod">A nibble-value that specifies the "encryptingMethod",
        ///	The value 0x0 specifies that no encryptingMethod is used.</param>
        /// <param name="MemoryAddress">starting address of server memory from which data is to be retrieved</param>
        /// <param name="MemoryAddressLength">Size in bytes of the MemoryAddress buffer (max.: 0xF)</param>
        /// <param name="MemorySize">used by the server to compare the uncompressed memory size with 
        ///	the total amount of data transferred during the TransferData service</param>
        /// <param name="MemorySizeLength">Size in bytes of the MemorySize buffer (max.: 0xF)</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestUpload")]
        public static extern TPUDSStatus SvcRequestUpload(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            byte CompressionMethod,
            byte EncryptingMethod,
            byte[] MemoryAddress,
            byte MemoryAddressLength,
            byte[] MemorySize,
            byte MemorySizeLength);
        #endregion

        #region UDS Service: TransferData
        // ISO-15765-3:2004 §9.7.1 p.57 && ISO-14229-1:2006 §14.4 p.237

        /// <summary>
        ///	The TransferData service is used by the client to transfer data either from the client 
        ///	to the server (download) or from the server to the client (upload).
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="BlockSequenceCounter">The blockSequenceCounter parameter value starts at 01 hex
        ///	with the first TransferData request that follows the RequestDownload (34 hex) 
        ///	or RequestUpload (35 hex) service. Its value is incremented by 1 for each subsequent
        ///	TransferData request. At the value of FF hex, the blockSequenceCounter rolls over 
        ///	and starts at 00 hex with the next TransferData request message.</param>
        /// <param name="Buffer">buffer containing the required transfer parameters</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcTransferData")]
        public static extern TPUDSStatus SvcTransferData(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            byte BlockSequenceCounter,
            byte[] Buffer,
            ushort BufferLength);
        #endregion

        #region UDS Service: RequestTransferExit
        // ISO-15765-3:2004 §9.7.1 p.57 && ISO-14229-1:2006 §14.5 p.242

        /// <summary>
        ///	The RequestTransferExit service is used by the client to terminate a data 
        ///	transfer between client and server (upload or download).
        /// </summary>
        /// <param name="CanChannel">A PUDS Channel Handle representing a PUDS-Client</param>
        /// <param name="MessageBuffer">The PUDS message (NO_POSITIVE_RESPONSE_MSG is ignored)</param>
        /// <param name="Buffer">buffer containing the required transfer parameters</param>
        /// <param name="BufferLength">Size in bytes of the buffer</param>
        /// <returns>A TPUDSStatus code. PUDS_ERROR_OK is returned on success</returns>
        [DllImport("PCAN-UDS.dll", EntryPoint = "UDS_SvcRequestTransferExit")]
        public static extern TPUDSStatus SvcRequestTransferExit(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            byte[] Buffer,
            ushort BufferLength);
        #endregion


        public static TPUDSStatus SvcReadDataByIdentifier_BypassPlugin(
            TPUDSCANHandle CanChannel,
            ref TPUDSMsg MessageBuffer,
            ushort[] Buffer,
            ushort BufferLength)
        {
            // Implementation of alternate SvcReadDataByIdentifier() using low level transfer
            // NOTE : Use of non-ISO 14229 service code!

            TPUDSStatus Status;

            MessageBuffer.DATA = new byte[4095];

            MessageBuffer.DATA[0] = 0x2D;       // Service ID. (non-ISO)

            for (uint n = 0; n < BufferLength; ++n)
            {
                MessageBuffer.DATA[1 + n * 2] = (byte)(Buffer[n] >> 8);
                MessageBuffer.DATA[2 + n * 2] = (byte)(Buffer[n]);
            }

            MessageBuffer.LEN = (ushort)(BufferLength * 2 + 1);
            MessageBuffer.MSGTYPE = TPUDSMessageType.PUDS_MESSAGE_TYPE_REQUEST;

            Status = UDSApi.Write(CanChannel, ref MessageBuffer);

            return Status;
        }

        #endregion
    }
    #endregion
}