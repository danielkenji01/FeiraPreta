USE [FeiraPreta]
GO
/****** Object:  Table [dbo].[EventScore]    Script Date: 02/03/2018 19:49:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventScore](
	[id] [uniqueidentifier] NOT NULL,
	[value] [float] NOT NULL,
	[createdDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_EventScore] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 02/03/2018 19:49:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[id] [uniqueidentifier] NOT NULL,
	[usernameInstagram] [varchar](50) NOT NULL,
	[fullNameInstagram] [varchar](200) NOT NULL,
	[createdDate] [datetime2](7) NOT NULL,
	[updatedDate] [datetime2](7) NULL,
	[profilePictureInstagram] [text] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Publication]    Script Date: 02/03/2018 19:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Publication](
	[id] [uniqueidentifier] NOT NULL,
	[personId] [uniqueidentifier] NOT NULL,
	[link] [text] NOT NULL,
	[createdDateInstagram] [datetime2](7) NOT NULL,
	[createdDate] [datetime2](7) NOT NULL,
	[updatedDate] [datetime2](7) NULL,
	[isHighlight] [bit] NOT NULL,
	[imageLowResolution] [text] NOT NULL,
	[imageThumbnail] [text] NOT NULL,
	[imageStandardResolution] [text] NOT NULL,
	[subtitle] [varchar](300) NULL,
 CONSTRAINT [PK_Publication] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Publication]  WITH CHECK ADD  CONSTRAINT [FK_Publication_Person] FOREIGN KEY([personId])
REFERENCES [dbo].[Person] ([id])
GO
ALTER TABLE [dbo].[Publication] CHECK CONSTRAINT [FK_Publication_Person]
GO
