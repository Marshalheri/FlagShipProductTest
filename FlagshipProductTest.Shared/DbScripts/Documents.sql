USE [FlagShipProductTestDB]
GO

/****** Object:  Table [dbo].[Documents]    Script Date: 2/21/2022 2:24:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Documents](
	[Id] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[DocumentType] [int] NOT NULL,
	[RawData] [nvarchar](max) NOT NULL,
	[Extension] [nvarchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

