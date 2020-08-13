CREATE TABLE [dbo].[ParkingTable](
	[ReceiptNumber] [int] IDENTITY(1,1) NOT NULL,
	[VehicalOwnerName] [varchar](30) NOT NULL,
	[VehicalOwnerEmail] [varchar](30) NOT NULL,
	[VehicalNumber] [varchar](30) NOT NULL,
	[Brand] [varchar](30) NOT NULL,
	[Color] [varchar](30) NOT NULL,
	[DriverName] [varchar](30) NOT NULL,
	[ParkingSlot] [varchar](30) NOT NULL,
	[Status] [varchar](30) NOT NULL,
	[IsHandicap] [bit] NULL,
	[ParkingDate] [varchar](30) NOT NULL,
	[UnparkDate] [varchar](30) NULL,
	[TotalTime] [varchar](30) NULL,
	[TotalAmount] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ReceiptNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ParkingTable] ADD  DEFAULT ((0)) FOR [IsHandicap]
GO

ALTER TABLE [dbo].[ParkingTable] ADD  DEFAULT (NULL) FOR [UnparkDate]
GO

ALTER TABLE [dbo].[ParkingTable] ADD  DEFAULT ((0)) FOR [TotalTime]
GO

ALTER TABLE [dbo].[ParkingTable] ADD  DEFAULT ((0)) FOR [TotalAmount]
GO
