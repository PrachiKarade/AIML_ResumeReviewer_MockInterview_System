USE [ResumeReviewer]
GO

/****** Object:  Table [dbo].[SystemPrompts]    Script Date: 1/12/2026 9:14:36 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SystemPrompts](
	[PromptId] [int] IDENTITY(1,1) NOT NULL,
	[ExperienceLevel] [nvarchar](50) NOT NULL,
	[Domain] [nvarchar](50) NULL,
	[Content] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PromptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SystemPrompts] ADD  DEFAULT ('Any') FOR [Domain]
GO

ALTER TABLE [dbo].[SystemPrompts] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO


