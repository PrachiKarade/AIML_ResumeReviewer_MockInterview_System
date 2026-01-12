USE [ResumeReviewer]
GO

/****** Object:  Table [dbo].[ResumeAnalysisPrompts]    Script Date: 1/12/2026 9:21:35 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ResumeAnalysisPrompts](
	[PromptId] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](50) NULL,
	[Content] [nvarchar](max) NULL,
	[Embedding] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[PromptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


