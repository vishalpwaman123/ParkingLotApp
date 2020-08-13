CREATE TABLE [dbo].[UserTable](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](30) NOT NULL,
	[LastName] [varchar](30) NOT NULL,
	[EmailId] [varchar](30) NOT NULL,
	[Role] [varchar](15) NOT NULL,
	[LocalAddress] [varchar](150) NOT NULL,
	[MobileNumber] [varchar](10) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[CreatedDate] [varchar](30) NOT NULL,
	[ModificationDate] [varchar](30) NOT NULL,
	[PresentState] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserTable] ADD  DEFAULT ((1)) FOR [PresentState]
GO
