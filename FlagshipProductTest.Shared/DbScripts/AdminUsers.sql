USE [FlagShipProductTestDB]
GO

/****** Object:  Table [dbo].[AdminUsers]    Script Date: 2/21/2022 2:25:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AdminUsers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[EmailAddress] [nvarchar](100) NOT NULL,
	[Gender] [int] NOT NULL,
	[Title] [int] NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[PasswordSalt] [nvarchar](max) NOT NULL,
	[FailedLoginCount] [int] NOT NULL,
	[ProfileStatus] [int] NOT NULL,
	[LastLogin] [datetime] NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_AdminUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

