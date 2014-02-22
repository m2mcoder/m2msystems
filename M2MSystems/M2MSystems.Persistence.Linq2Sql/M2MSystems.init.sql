USE [M2MSystems]
GO

/****** Object:  Table [dbo].[Products]    Script Date: 2/22/2014 9:19:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Products](
	[id] [bigint] identity(1,1) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Products] ON
GO

CREATE TABLE [dbo].[Applicants](
	[id] [bigint] identity(1,1) NOT NULL,
	[firstname] [nvarchar](50) NULL,
	[lastname] [nvarchar](50) NULL,
	[address1] [nvarchar](50) NULL,
	[address2] [nvarchar](50) NULL,
	[city] [nvarchar](50) NULL,
	[thirdpartykey] [nvarchar](50) NULL,
	[email] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Applicants] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Applicants] ON
GO

CREATE TABLE [dbo].[Forms](
	[id] [bigint] identity(1,1) NOT NULL,
	[productid] [bigint] NOT NULL,
	[title] [nvarchar](200) NOT NULL,
	[applicationextractor] [varchar](20) NOT NULL,
	[feecalculator] [varchar](50) NOT NULL,
	[url] [varchar](200) NOT NULL,
 CONSTRAINT [PK_Forms] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Forms] ON
GO

ALTER TABLE [dbo].[Forms]  WITH CHECK ADD  CONSTRAINT [FK_Forms_Products] FOREIGN KEY([productid])
REFERENCES [dbo].[Products] ([id])
GO

ALTER TABLE [dbo].[Forms] CHECK CONSTRAINT [FK_Forms_Products]
GO


CREATE TABLE [dbo].[Insurers](
	[id] [bigint] identity(1,1) NOT NULL,
	[name] [varchar](200) NOT NULL,
 CONSTRAINT [PK_Insurers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Insurers] ON
GO


CREATE TABLE [dbo].[Documents](
	[id] [bigint] identity(1,1) NOT NULL,
	[bin] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Documents] ON
GO

CREATE TABLE [dbo].[Policies](
	[id] [bigint] identity(1,1) NOT NULL,
	[productid] [bigint] NOT NULL,
	[cost] [numeric](18, 2) NOT NULL,
	[enddate] [datetimeoffset](7) NOT NULL,
	[startdate] [datetimeoffset](7) NOT NULL,
	[documentid] [bigint] NOT NULL,
 CONSTRAINT [PK_Policies] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Policies]  WITH CHECK ADD  CONSTRAINT [FK_Policies_Documents] FOREIGN KEY([documentid])
REFERENCES [dbo].[Documents] ([id])
GO

ALTER TABLE [dbo].[Policies] CHECK CONSTRAINT [FK_Policies_Documents]
GO

ALTER TABLE [dbo].[Policies]  WITH CHECK ADD  CONSTRAINT [FK_Policies_Products] FOREIGN KEY([productid])
REFERENCES [dbo].[Products] ([id])
GO

ALTER TABLE [dbo].[Policies] CHECK CONSTRAINT [FK_Policies_Products]
GO

SET IDENTITY_INSERT [dbo].[Policies] ON
GO

CREATE TABLE [dbo].[SavedForms](
	[id] [bigint] identity(1,1) NOT NULL,
	[applicantid] [bigint] NOT NULL,
	[formid] [bigint] NOT NULL,
	[policyid] [bigint] NULL,
	[answers] [varchar](max) NOT NULL,
 CONSTRAINT [PK_SavedForms] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[SavedForms] ON
GO


ALTER TABLE [dbo].[SavedForms]  WITH CHECK ADD  CONSTRAINT [FK_SavedForms_Applicants] FOREIGN KEY([applicantid])
REFERENCES [dbo].[Applicants] ([id])
GO

ALTER TABLE [dbo].[SavedForms] CHECK CONSTRAINT [FK_SavedForms_Applicants]
GO

ALTER TABLE [dbo].[SavedForms]  WITH CHECK ADD  CONSTRAINT [FK_SavedForms_Forms] FOREIGN KEY([formid])
REFERENCES [dbo].[Forms] ([id])
GO

ALTER TABLE [dbo].[SavedForms] CHECK CONSTRAINT [FK_SavedForms_Forms]
GO

ALTER TABLE [dbo].[SavedForms]  WITH CHECK ADD  CONSTRAINT [FK_SavedForms_Policies] FOREIGN KEY([policyid])
REFERENCES [dbo].[Policies] ([id])
GO

ALTER TABLE [dbo].[SavedForms] CHECK CONSTRAINT [FK_SavedForms_Policies]
GO

CREATE TABLE [dbo].[Partners](
	[id] [bigint] identity(1,1) NOT NULL,
	[rate] [numeric](18, 6) NOT NULL,
	[Name] [varchar](200) NOT NULL,
 CONSTRAINT [PK_Partners] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Partners] ON
GO

CREATE TABLE [dbo].[InsurerPartners](
	[id] [bigint] identity(1,1) NOT NULL,
	[insurerid] [bigint] NOT NULL,
	[partnerid] [bigint] NOT NULL,
	[commissionrate] [numeric](18, 6) NOT NULL,
 CONSTRAINT [PK_InsurerPartners] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[InsurerPartners]  WITH CHECK ADD  CONSTRAINT [FK_InsurerPartners_Insurers] FOREIGN KEY([insurerid])
REFERENCES [dbo].[Insurers] ([id])
GO

ALTER TABLE [dbo].[InsurerPartners] CHECK CONSTRAINT [FK_InsurerPartners_Insurers]
GO

ALTER TABLE [dbo].[InsurerPartners]  WITH CHECK ADD  CONSTRAINT [FK_InsurerPartners_Partners] FOREIGN KEY([partnerid])
REFERENCES [dbo].[Partners] ([id])
GO

ALTER TABLE [dbo].[InsurerPartners] CHECK CONSTRAINT [FK_InsurerPartners_Partners]
GO

SET IDENTITY_INSERT [dbo].[InsurerPartners] ON
GO

CREATE TABLE [dbo].[InsurerPartnerProducts](
	[id] [bigint] identity(1,1) NOT NULL,
	[insurerpartnerid] [bigint] NOT NULL,
	[productid] [bigint] NOT NULL,
	[commissionrate] [numeric](18, 6) NOT NULL,
 CONSTRAINT [PK_InsurerPartnerProducts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[InsurerPartnerProducts]  WITH CHECK ADD  CONSTRAINT [FK_InsurerPartnerProducts_InsurerPartners] FOREIGN KEY([insurerpartnerid])
REFERENCES [dbo].[InsurerPartners] ([id])
GO

ALTER TABLE [dbo].[InsurerPartnerProducts] CHECK CONSTRAINT [FK_InsurerPartnerProducts_InsurerPartners]
GO

ALTER TABLE [dbo].[InsurerPartnerProducts]  WITH CHECK ADD  CONSTRAINT [FK_InsurerPartnerProducts_Products] FOREIGN KEY([productid])
REFERENCES [dbo].[Products] ([id])
GO

ALTER TABLE [dbo].[InsurerPartnerProducts] CHECK CONSTRAINT [FK_InsurerPartnerProducts_Products]
GO

SET IDENTITY_INSERT [dbo].[InsurerPartnerProducts] ON
GO



