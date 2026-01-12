USE [ResumeReviewer]
GO

/****** Object:  Table [dbo].[ResumeChunks]    Script Date: 1/12/2026 9:12:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ResumeChunks](
	[ChunkId] [int] IDENTITY(1,1) NOT NULL,
	[ResumeId] [int] NULL,
	[ChunkText] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ChunkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ResumeChunks] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO


