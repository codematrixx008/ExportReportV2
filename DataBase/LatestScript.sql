USE [Record]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reports]') AND type in (N'U'))
ALTER TABLE [dbo].[Reports] DROP CONSTRAINT IF EXISTS [DF__Reports__IsGener__51300E55]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reports]') AND type in (N'U'))
ALTER TABLE [dbo].[Reports] DROP CONSTRAINT IF EXISTS [DF__Reports__IsGener__503BEA1C]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reports]') AND type in (N'U'))
ALTER TABLE [dbo].[Reports] DROP CONSTRAINT IF EXISTS [DF__Reports__HasStat__4F47C5E3]
GO
/****** Object:  Table [dbo].[Reports]    Script Date: 04/01/2025 3:16:40 PM ******/
DROP TABLE IF EXISTS [dbo].[Reports]
GO
/****** Object:  Table [dbo].[Reports]    Script Date: 04/01/2025 3:16:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[ReportId] [int] NOT NULL,
	[ReportName] [varchar](255) NULL,
	[SPName] [varchar](255) NULL,
	[LastGeneratedOn] [datetime] NULL,
	[LastGeneratedBy] [int] NULL,
	[HasStaticFile] [bit] NOT NULL,
	[IsGenerating] [bit] NOT NULL,
	[IsGenerated] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (1, N'1 Lakh', N'sp_Get1LakhRecord', CAST(N'2025-03-31T16:09:32.220' AS DateTime), 123, 1, 0, 1)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (2, N'2 Lakh', N'sp_Get2LakhRecord', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (3, N'5 Lakh', N'sp_Get5LakhRecord', CAST(N'2025-03-31T11:22:42.737' AS DateTime), 123, 1, 0, 1)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (4, N'10 Lakh', N'sp_Get10LakhRecord', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 1, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (5, N'Sales Report', N'sp_GetSalesReport', CAST(N'2025-03-31T11:14:14.673' AS DateTime), 123, 1, 0, 1)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (6, N'Inventory Report', N'sp_GetInventoryReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 1, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (7, N'Customer Report', N'sp_GetCustomerReport', CAST(N'2025-03-31T11:18:59.673' AS DateTime), 123, 1, 0, 1)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (8, N'Employee Report', N'sp_GetEmployeeReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (9, N'Expense Report', N'sp_GetExpenseReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (10, N'Revenue Report', N'sp_GetRevenueReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (11, N'Profit Report', N'sp_GetProfitReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (12, N'Product Performance Report', N'sp_GetProductPerformanceReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (13, N'Marketing Report', N'sp_GetMarketingReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (14, N'Finance Report', N'sp_GetFinanceReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (15, N'Order Report', N'sp_GetOrderReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (16, N'Shipping Report', N'sp_GetShippingReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (17, N'Supplier Report', N'sp_GetSupplierReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (18, N'HR Report', N'sp_GetHRReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (19, N'Attendance Report', N'sp_GetAttendanceReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (20, N'Payroll Report', N'sp_GetPayrollReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (21, N'User Activity Report', N'sp_GetUserActivityReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (22, N'Security Report', N'sp_GetSecurityReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (23, N'Compliance Report', N'sp_GetComplianceReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (24, N'IT Asset Report', N'sp_GetITAssetReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (25, N'Budget Report', N'sp_GetBudgetReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (26, N'Training Report', N'sp_GetTrainingReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (27, N'Audit Report', N'sp_GetAuditReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (28, N'Growth Report', N'sp_GetGrowthReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
INSERT [dbo].[Reports] ([ReportId], [ReportName], [SPName], [LastGeneratedOn], [LastGeneratedBy], [HasStaticFile], [IsGenerating], [IsGenerated]) VALUES (29, N'Productivity Report', N'sp_GetProductivityReport', CAST(N'2025-03-28T16:19:07.733' AS DateTime), 0, 0, 0, 0)
GO
ALTER TABLE [dbo].[Reports] ADD  DEFAULT ((0)) FOR [HasStaticFile]
GO
ALTER TABLE [dbo].[Reports] ADD  DEFAULT ((0)) FOR [IsGenerating]
GO
ALTER TABLE [dbo].[Reports] ADD  DEFAULT ((0)) FOR [IsGenerated]
GO
