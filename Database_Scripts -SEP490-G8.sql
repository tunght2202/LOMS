USE [LOMSDATABASE]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
        [MigrationId] [nvarchar](150) NOT NULL,
        [ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
        [MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
        [CommentID] [nvarchar](450) NOT NULL,
        [Content] [nvarchar](max) NOT NULL,
        [CommentTime] [datetime2](7) NOT NULL,
        [LiveStreamCustomerID] [int] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
        [CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
        [CustomerID] [nvarchar](450) NOT NULL,
        [FacebookName] [nvarchar](255) NOT NULL,
        [ImageURL] [nvarchar](max) NULL,
        [FullName] [nvarchar](max) NULL,
        [PhoneNumber] [nvarchar](20) NULL,
        [Email] [nvarchar](255) NULL,
        [Address] [nvarchar](max) NULL,
        [SuccessfulDeliveries] [int] NOT NULL,
        [FailedDeliveries] [int] NOT NULL,
        [CreatedAt] [datetime2](7) NOT NULL,
        [UpdatedAt] [datetime2](7) NOT NULL,
        [Status] [bit] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
        [CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ListProducts]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListProducts](
        [ListProductId] [int] IDENTITY(1,1) NOT NULL,
        [UserID] [nvarchar](450) NOT NULL,
        [ListProductName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ListProducts] PRIMARY KEY CLUSTERED 
(
        [ListProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LiveStreamCustomers]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LiveStreamCustomers](
        [LiveStreamCustomerId] [int] IDENTITY(1,1) NOT NULL,
        [LivestreamID] [nvarchar](450) NOT NULL,
        [CustomerID] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_LiveStreamCustomers] PRIMARY KEY CLUSTERED 
(
        [LiveStreamCustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LiveStreams]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LiveStreams](
        [LivestreamID] [nvarchar](450) NOT NULL,
        [UserID] [nvarchar](450) NOT NULL,
        [ListProductID] [int] NULL,
        [StreamURL] [nvarchar](max) NOT NULL,
        [StreamTitle] [nvarchar](max) NOT NULL,
        [StartTime] [datetime2](7) NOT NULL,
        [Status] [nvarchar](max) NOT NULL,
        [StatusDelete] [bit] NOT NULL,
        [PriceMax] [decimal](18, 2) NULL,
 CONSTRAINT [PK_LiveStreams] PRIMARY KEY CLUSTERED 
(
        [LivestreamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
        [OrderID] [int] IDENTITY(1,1) NOT NULL,
        [OrderDate] [datetime2](7) NOT NULL,
        [Status] [int] NOT NULL,
        [Quantity] [int] NOT NULL,
        [ProductID] [int] NOT NULL,
        [CommentID] [nvarchar](450) NULL,
        [CurrentPrice] [decimal](18, 2) NULL,
        [Note] [nvarchar](max) NULL,
        [StatusCheck] [bit] NOT NULL,
        [TrackingNumber] [nvarchar](max) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
        [OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductListProducts]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductListProducts](
        [ProductListProductID] [int] IDENTITY(1,1) NOT NULL,
        [ProductID] [int] NOT NULL,
        [ListProductID] [int] NOT NULL,
 CONSTRAINT [PK_ProductListProducts] PRIMARY KEY CLUSTERED 
(
        [ProductListProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
        [ProductID] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NOT NULL,
        [ProductCode] [nvarchar](max) NULL,
        [ImageURL] [nvarchar](max) NULL,
        [Description] [nvarchar](max) NULL,
        [UserID] [nvarchar](450) NOT NULL,
        [Price] [decimal](18, 2) NOT NULL,
        [Stock] [int] NOT NULL,
        [Status] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
        [ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleClaims]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleClaims](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [RoleId] [nvarchar](450) NOT NULL,
        [ClaimType] [nvarchar](max) NULL,
        [ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_RoleClaims] PRIMARY KEY CLUSTERED 
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
        [Id] [nvarchar](450) NOT NULL,
        [Name] [nvarchar](256) NULL,
        [NormalizedName] [nvarchar](256) NULL,
        [ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserClaims]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClaims](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [UserId] [nvarchar](450) NOT NULL,
        [ClaimType] [nvarchar](max) NULL,
        [ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserClaims] PRIMARY KEY CLUSTERED 
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLogins]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogins](
        [LoginProvider] [nvarchar](450) NOT NULL,
        [ProviderKey] [nvarchar](450) NOT NULL,
        [ProviderDisplayName] [nvarchar](max) NULL,
        [UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_UserLogins] PRIMARY KEY CLUSTERED 
(
        [LoginProvider] ASC,
        [ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
        [UserId] [nvarchar](450) NOT NULL,
        [RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
        [UserId] ASC,
        [RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
        [Id] [nvarchar](450) NOT NULL,
        [CreatedAt] [datetime2](7) NOT NULL,
        [ImageURL] [nvarchar](max) NULL,
        [FullName] [nvarchar](max) NULL,
        [Sex] [nvarchar](max) NULL,
        [Address] [nvarchar](max) NULL,
        [TokenFacbook] [nvarchar](max) NULL,
        [PageId] [nvarchar](max) NULL,
        [UserName] [nvarchar](256) NULL,
        [NormalizedUserName] [nvarchar](256) NULL,
        [Email] [nvarchar](256) NULL,
        [NormalizedEmail] [nvarchar](256) NULL,
        [EmailConfirmed] [bit] NOT NULL,
        [PasswordHash] [nvarchar](max) NULL,
        [SecurityStamp] [nvarchar](max) NULL,
        [ConcurrencyStamp] [nvarchar](max) NULL,
        [PhoneNumber] [nvarchar](max) NULL,
        [PhoneNumberConfirmed] [bit] NOT NULL,
        [TwoFactorEnabled] [bit] NOT NULL,
        [LockoutEnd] [datetimeoffset](7) NULL,
        [LockoutEnabled] [bit] NOT NULL,
        [AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTokens]    Script Date: 29/04/2025 1:08:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTokens](
        [UserId] [nvarchar](450) NOT NULL,
        [LoginProvider] [nvarchar](450) NOT NULL,
        [Name] [nvarchar](450) NOT NULL,
        [Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserTokens] PRIMARY KEY CLUSTERED 
(
        [UserId] ASC,
        [LoginProvider] ASC,
        [Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250422061421_Init', N'9.0.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250428140244_updateOrderandLiveStream', N'9.0.2')
GO
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1008609687603535_1035650668501808', N'pdt1x', CAST(N'2025-04-25T18:00:13.0000000' AS DateTime2), 48)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1008609687603535_1175003657705261', N'123x100', CAST(N'2025-04-25T17:58:10.0000000' AS DateTime2), 48)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1008609687603535_1339949637265955', N'123', CAST(N'2025-04-25T17:27:53.0000000' AS DateTime2), 48)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1008609687603535_1433100297674777', N'pdt1x1', CAST(N'2025-04-25T18:00:19.0000000' AS DateTime2), 48)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1008609687603535_1600036030708786', N'123', CAST(N'2025-04-25T17:58:00.0000000' AS DateTime2), 48)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1008609687603535_658745653556318', N'b111', CAST(N'2025-04-25T17:28:12.0000000' AS DateTime2), 48)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1008609687603535_665580122945248', N'123 x100', CAST(N'2025-04-25T17:59:54.0000000' AS DateTime2), 48)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1052892650035808_1181612027313660', N'chào bạn', CAST(N'2025-04-24T14:21:22.0000000' AS DateTime2), 24)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1052892650035808_1440391603790833', N'A002', CAST(N'2025-04-24T14:21:09.0000000' AS DateTime2), 11)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1052892650035808_1584899995520942', N'A003', CAST(N'2025-04-24T14:22:15.0000000' AS DateTime2), 12)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1052892650035808_2502069463495085', N'Hello', CAST(N'2025-04-24T14:20:58.0000000' AS DateTime2), 12)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1052892650035808_547277658429186', N'Chào bạn', CAST(N'2025-04-24T14:20:54.0000000' AS DateTime2), 11)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1052892650035808_667912689281339', N'a003', CAST(N'2025-04-24T14:21:27.0000000' AS DateTime2), 13)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1052892650035808_695125879769452', N'Tôi muốn muaa', CAST(N'2025-04-24T14:21:25.0000000' AS DateTime2), 12)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1052892650035808_698403256100397', N'Chàoo', CAST(N'2025-04-24T14:21:12.0000000' AS DateTime2), 12)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1127442832480132_1192451899185525', N'P002', CAST(N'2025-04-24T09:28:10.0000000' AS DateTime2), 8)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1127442832480132_558250130641042', N'P001', CAST(N'2025-04-24T09:29:15.0000000' AS DateTime2), 8)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1127442832480132_656671783650618', N'P005', CAST(N'2025-04-24T09:28:27.0000000' AS DateTime2), 8)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1127442832480132_687462463645413', N'P001', CAST(N'2025-04-24T09:27:56.0000000' AS DateTime2), 8)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1180235143794968_1006317085019027', N'hello cả nhà', CAST(N'2025-04-26T21:46:39.0000000' AS DateTime2), 71)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1180235143794968_1047139427278018', N'Btl150', CAST(N'2025-04-26T21:59:50.0000000' AS DateTime2), 72)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1214821850656120_1856811988410061', N'hrlllo', CAST(N'2025-04-23T15:22:07.0000000' AS DateTime2), 6)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1214821850656120_675841108360277', N'p001', CAST(N'2025-04-23T15:22:21.0000000' AS DateTime2), 6)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1320231362386280_2716139045238562', N'sa200 -400000', CAST(N'2025-04-26T23:09:43.0000000' AS DateTime2), 88)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1165533981868703', N'brbtnmm', CAST(N'2025-01-16T14:48:31.0000000' AS DateTime2), 61)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1192413555867802', N'pdt38', CAST(N'2025-04-10T15:58:58.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1353870032517823', N'pdt4 40', CAST(N'2025-04-10T15:52:19.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1354668375806883', N'pdt4 35', CAST(N'2025-04-10T15:52:29.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1383978676067915', N'111', CAST(N'2025-04-21T16:54:54.0000000' AS DateTime2), 64)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1702140957039604', N'pdt4 40', CAST(N'2025-04-10T15:52:25.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1797795457465440', N'abc', CAST(N'2025-04-21T16:51:50.0000000' AS DateTime2), 64)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1828690240999703', N'dhaaa', CAST(N'2025-01-16T21:14:40.0000000' AS DateTime2), 63)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1832869977480139', N'pdt4 3', CAST(N'2025-04-10T15:52:35.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_1896357384525602', N'pdt5 5', CAST(N'2025-04-14T17:00:53.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_2985784391580226', N'pdt4', CAST(N'2025-04-10T15:50:56.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_3536597633300095', N'lẫn tan', CAST(N'2025-01-16T14:55:39.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_4026310370921609', N'cong an  danh dan', CAST(N'2025-01-16T14:49:10.0000000' AS DateTime2), 60)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_495774880156012', N'pdt5 5', CAST(N'2025-04-10T16:04:11.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_540509652051508', N'pdt34', CAST(N'2025-04-10T15:58:53.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_561821153595935', N'pdt54', CAST(N'2025-04-10T16:00:32.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_594216920207853', N'xin chào', CAST(N'2025-01-16T14:54:37.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_636302025723984', N'chốt cái đàn', CAST(N'2025-01-16T14:48:38.0000000' AS DateTime2), 60)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_641679245449878', N'hihi', CAST(N'2025-04-14T15:01:41.0000000' AS DateTime2), 64)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_9033700203389152', N'HI MỌI NGƯỜI', CAST(N'2025-01-16T14:48:09.0000000' AS DateTime2), 59)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_924877036435113', N'br', CAST(N'2025-01-16T14:48:46.0000000' AS DateTime2), 61)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1347523583273065_9471549939548571', N'pdt4 1', CAST(N'2025-04-10T15:51:02.0000000' AS DateTime2), 62)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1361375685178654_1433181768060937', N'100', CAST(N'2025-04-24T22:40:38.0000000' AS DateTime2), 32)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1361375685178654_1686841432222128', N'100 x sl', CAST(N'2025-04-24T22:40:22.0000000' AS DateTime2), 31)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1362899964856870_1432687768104769', N'a002', CAST(N'2025-04-25T16:47:20.0000000' AS DateTime2), 44)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1362899964856870_1650865072218524', N'a001', CAST(N'2025-04-25T16:46:40.0000000' AS DateTime2), 44)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1362899964856870_2446379882388799', N'a001', CAST(N'2025-04-25T16:56:27.0000000' AS DateTime2), 45)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1362899964856870_986953920177435', N'A001', CAST(N'2025-04-25T16:47:12.0000000' AS DateTime2), 43)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1362899964856870_990731693224774', N'A002', CAST(N'2025-04-25T16:46:31.0000000' AS DateTime2), 43)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1373913337259713_1808501316378964', N'P002', CAST(N'2025-04-23T15:26:13.0000000' AS DateTime2), 7)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1373913337259713_24212677751654942', N'P003', CAST(N'2025-04-23T15:26:31.0000000' AS DateTime2), 7)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1373913337259713_3878349682428899', N'P001', CAST(N'2025-04-23T15:26:11.0000000' AS DateTime2), 7)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1373913337259713_545858178563059', N'P001', CAST(N'2025-04-23T15:26:18.0000000' AS DateTime2), 7)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_1005214661715695', N'A005', CAST(N'2025-04-25T14:09:48.0000000' AS DateTime2), 35)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_1022060326547223', N'A100', CAST(N'2025-04-24T22:50:56.0000000' AS DateTime2), 33)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_1179618506733277', N'003', CAST(N'2025-04-25T14:13:06.0000000' AS DateTime2), 35)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_1206277763693951', N'100', CAST(N'2025-04-24T22:45:07.0000000' AS DateTime2), 33)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_1254526519618560', N'100', CAST(N'2025-04-24T22:49:44.0000000' AS DateTime2), 33)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_1780980042631357', N'A002', CAST(N'2025-04-25T14:09:27.0000000' AS DateTime2), 36)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_2417813438573275', N'100 x4', CAST(N'2025-04-24T23:14:38.0000000' AS DateTime2), 34)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_3624741914491081', N'A004', CAST(N'2025-04-24T22:47:33.0000000' AS DateTime2), 33)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_525660110599670', N'100', CAST(N'2025-04-24T22:42:41.0000000' AS DateTime2), 33)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_640930882095958', N'a100', CAST(N'2025-04-24T23:14:57.0000000' AS DateTime2), 34)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1405707520555010_995047289402438', N'100', CAST(N'2025-04-24T23:14:47.0000000' AS DateTime2), 34)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1439825920702639_1216372820115820', N'sa200 100000', CAST(N'2025-04-26T23:04:57.0000000' AS DateTime2), 87)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1439825920702639_1231956698321556', N'sa200 4', CAST(N'2025-04-26T23:04:27.0000000' AS DateTime2), 87)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1439825920702639_1386889072450459', N'sa200 2', CAST(N'2025-04-26T23:05:03.0000000' AS DateTime2), 87)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1439825920702639_2067392260424263', N'SA200 -1000', CAST(N'2025-04-26T23:03:08.0000000' AS DateTime2), 87)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1439825920702639_4113060352265155', N'sa200 -10000000', CAST(N'2025-04-26T23:03:51.0000000' AS DateTime2), 87)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1439825920702639_458316700671986', N'sa200 100000', CAST(N'2025-04-26T23:04:47.0000000' AS DateTime2), 87)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_1040001291370800', N'Hello shop', CAST(N'2025-04-25T23:21:38.0000000' AS DateTime2), 54)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_1189014142973967', N'giới thiệu đi sốp', CAST(N'2025-04-25T23:23:15.0000000' AS DateTime2), 52)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_1204783174675704', N'abc', CAST(N'2025-04-25T23:05:29.0000000' AS DateTime2), 52)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_1213941247059522', N'giới thieju di doi lau qua sop', CAST(N'2025-04-25T23:23:38.0000000' AS DateTime2), 52)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_1406473380353983', N'Chào shop nha', CAST(N'2025-04-25T23:21:05.0000000' AS DateTime2), 53)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_4105828436314406', N'Alo', CAST(N'2025-04-25T23:07:49.0000000' AS DateTime2), 53)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_638314422364388', N'chào shop nhá', CAST(N'2025-04-25T23:20:25.0000000' AS DateTime2), 52)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_639406885921756', N'Shop nay có sản phẩm j ạ', CAST(N'2025-04-25T23:21:22.0000000' AS DateTime2), 53)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_651422491195888', N'abc', CAST(N'2025-04-25T23:05:27.0000000' AS DateTime2), 52)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_654132244161695', N'💬 Comment theo cú pháp:
👉 Mã sản phẩm + số lượng
🔖 Ví dụ: A001 x2 (mua 2 sản phẩm mã A001)


📩 Sau khi comment, nhớ NHẮN TIN TRƯỚC cho fanpage để được chốt đơn nhanh và giữ hàng nha cả nhà ơi!', CAST(N'2025-04-25T23:05:06.0000000' AS DateTime2), 51)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1592503564702625_704088435415244', N'lên sản phẩm chứ sốp', CAST(N'2025-04-25T23:22:02.0000000' AS DateTime2), 52)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_1207890507788282', N'Btl150', CAST(N'2025-04-26T22:04:16.0000000' AS DateTime2), 73)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_1218174999683890', N'dùng như nào thế shop', CAST(N'2025-04-26T22:05:26.0000000' AS DateTime2), 75)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_1241758444238999', N'💬 Comment theo cú pháp:
 👉 Mã sản phẩm + số lượng
 🔖 Ví dụ: A001 x2 (mua 2 sản phẩm mã A001)
📩 Sau khi comment, nhớ NHẮN TIN TRƯỚC cho fanpage để được chốt đơn nhanh và giữ hàng nha cả nhà ơi!', CAST(N'2025-04-26T22:02:46.0000000' AS DateTime2), 74)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_1348769099742294', N'Shop nói hay quá', CAST(N'2025-04-26T22:03:32.0000000' AS DateTime2), 73)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_1352503329320903', N'BTL150 3', CAST(N'2025-04-26T22:06:13.0000000' AS DateTime2), 78)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_1600155720662603', N'Btl150', CAST(N'2025-04-26T22:05:32.0000000' AS DateTime2), 73)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_1838890626835394', N'Mã là gì vậy ạ', CAST(N'2025-04-26T22:05:23.0000000' AS DateTime2), 73)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_2170786910021680', N'Btl150 2', CAST(N'2025-04-26T22:06:03.0000000' AS DateTime2), 76)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_4043132089298931', N'các bạn, mn', CAST(N'2025-04-26T22:05:04.0000000' AS DateTime2), 77)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_649868337911936', N'.', CAST(N'2025-04-26T22:03:33.0000000' AS DateTime2), 76)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_680505914916479', N'btl150 x100', CAST(N'2025-04-26T22:05:42.0000000' AS DateTime2), 75)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_703507065479981', N'Chào shoppp', CAST(N'2025-04-26T22:02:50.0000000' AS DateTime2), 73)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_709972101599541', N'tắt meet', CAST(N'2025-04-26T22:04:01.0000000' AS DateTime2), 77)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1720448455516502_9784006121715074', N'chào shopppp', CAST(N'2025-04-26T22:03:09.0000000' AS DateTime2), 75)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1019101313078187', N'ODT300', CAST(N'2025-04-28T14:36:50.0000000' AS DateTime2), 1072)
GO
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1064457672415294', N'odt300 -1000', CAST(N'2025-04-28T14:59:40.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1102226815280766', N'odt300 x1', CAST(N'2025-04-28T14:59:02.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1184718916458194', N'ODT300 1', CAST(N'2025-04-28T14:37:22.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1202261908263204', N'odt300 1@#', CAST(N'2025-04-28T14:58:42.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1219166823170300', N'odt300 -1000', CAST(N'2025-04-28T15:14:08.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1227926182327989', N'odt300 1@', CAST(N'2025-04-28T15:14:15.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1343001090247122', N'odt -2000000', CAST(N'2025-04-28T14:39:58.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1366657884376883', N'👉 Mã sản phẩm + số lượng
 🔖 Ví dụ: A001 x2 (mua 2 sản phẩm mã A001)', CAST(N'2025-04-28T14:30:54.0000000' AS DateTime2), 1073)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1441051080195994', N'odt300 -1', CAST(N'2025-04-28T15:05:28.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1475118363896312', N'odt300 2', CAST(N'2025-04-28T14:37:49.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1483632272606369', N'đặt hàng thế nào ạ?', CAST(N'2025-04-28T14:34:49.0000000' AS DateTime2), 1074)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1494194584886705', N'Nay shop bán gì thế', CAST(N'2025-04-28T14:33:51.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1595524597786466', N'odt300', CAST(N'2025-04-28T15:13:07.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1669081393722384', N'Hello shop', CAST(N'2025-04-28T14:30:09.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1697661977515882', N'odt300 1 1', CAST(N'2025-04-28T15:16:09.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1729848510945268', N'odt300 1@', CAST(N'2025-04-28T15:05:37.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1742481276305491', N'odt300 1g', CAST(N'2025-04-28T14:46:29.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1771969486997522', N'odt hehe', CAST(N'2025-04-28T14:37:41.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1778339663092605', N'odt300 h1', CAST(N'2025-04-28T15:14:29.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1813949909176979', N'odt300', CAST(N'2025-04-28T14:36:02.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1841318873375717', N'odt 300', CAST(N'2025-04-28T15:00:00.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1903677717112059', N'giá bao nhiêu b ơi', CAST(N'2025-04-28T14:37:24.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_1904181346989434', N'odt300 1//', CAST(N'2025-04-28T14:44:50.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_2111255109389388', N'bL700 3', CAST(N'2025-04-28T14:38:40.0000000' AS DateTime2), 1074)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_23927200806873885', N'odt300 odt300 1', CAST(N'2025-04-28T15:13:41.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_2523168651352732', N'A200', CAST(N'2025-04-28T14:35:12.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_3041522006014166', N'ông gim tin nhắn', CAST(N'2025-04-28T14:30:31.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_3575052256124383', N'odt300 01', CAST(N'2025-04-28T15:00:42.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_4166475213591753', N'odt 100', CAST(N'2025-04-28T14:37:52.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_4207097396216605', N'odt300 odt300 2\', CAST(N'2025-04-28T14:43:03.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_4376002362622806', N'odt300 -1', CAST(N'2025-04-28T14:54:54.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_472979765842415', N'chào shop nha', CAST(N'2025-04-28T14:29:12.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_511195548738589', N'odt300 /1', CAST(N'2025-04-28T14:47:11.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_533925533106833', N'odt300 odt300 2', CAST(N'2025-04-28T14:43:08.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_604009015978464', N'odt300 odt300 1', CAST(N'2025-04-28T14:41:50.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_608612651638042', N'ODT300 2', CAST(N'2025-04-28T14:37:24.0000000' AS DateTime2), 1074)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_613941651806145', N'odt odt300 1', CAST(N'2025-04-28T15:13:27.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_621155920962589', N'odt300 -10000', CAST(N'2025-04-28T14:36:14.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_638348125685223', N'odt300 1@', CAST(N'2025-04-28T14:54:06.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_653230317475165', N'odt300 1', CAST(N'2025-04-28T14:36:06.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_655861043982542', N'odt300 odt300 1', CAST(N'2025-04-28T14:54:19.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_669538159018377', N'odt300 01', CAST(N'2025-04-28T15:13:54.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_676958061958483', N'odt300 1h', CAST(N'2025-04-28T15:14:36.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_683649037936086', N'odt300 10000000', CAST(N'2025-04-28T14:36:28.0000000' AS DateTime2), 1074)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_687080473868806', N'odt300 odt300 1', CAST(N'2025-04-28T15:04:59.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_690338970316077', N'odt300 01', CAST(N'2025-04-28T15:00:50.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_697114196160978', N'odt300  h1', CAST(N'2025-04-28T14:47:02.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_704524932265825', N'odt300 1+1', CAST(N'2025-04-28T15:15:26.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_705302018735280', N'mình hoàn hàng nếu k ưng được k bạn', CAST(N'2025-04-28T14:36:20.0000000' AS DateTime2), 1074)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_843939201287412', N'odt300 1%', CAST(N'2025-04-28T14:45:17.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_943499351051055', N'odt300 10000', CAST(N'2025-04-28T14:36:19.0000000' AS DateTime2), 1071)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_9549415218445903', N'odt 10000', CAST(N'2025-04-28T14:40:11.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_9597063643717651', N'odt300 -10000', CAST(N'2025-04-28T14:54:45.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_984033153893062', N'odt300 1', CAST(N'2025-04-28T15:15:02.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1721266082076842_999665482347508', N'odt300 1@#$%%', CAST(N'2025-04-28T14:47:42.0000000' AS DateTime2), 1072)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_10057381867659118', N'1', CAST(N'2025-04-26T00:42:03.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1013278550780684', N'2', CAST(N'2025-04-26T00:45:15.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1026807298895011', N'1', CAST(N'2025-04-26T00:43:03.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1043749721151961', N'2', CAST(N'2025-04-26T00:45:14.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1046505570727294', N'1', CAST(N'2025-04-26T00:42:44.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1048732583789797', N'1', CAST(N'2025-04-26T00:43:02.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1102715348558119', N'BTL001', CAST(N'2025-04-26T00:44:08.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1129780389186770', N'2', CAST(N'2025-04-26T00:45:15.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1138020724676145', N'bán đi sốp', CAST(N'2025-04-26T00:40:08.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1185730479691095', N'2', CAST(N'2025-04-26T00:45:12.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1196203105626882', N'💬 Comment theo cú pháp:
👉 Mã sản phẩm + số lượng
🔖 Ví dụ: A001 x2 (mua 2 sản phẩm mã A001)


📩 Sau khi comment, nhớ NHẮN TIN TRƯỚC cho fanpage để được chốt đơn nhanh và giữ hàng nha cả nhà ơi!', CAST(N'2025-04-26T00:40:32.0000000' AS DateTime2), 68)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1196975348734597', N'1', CAST(N'2025-04-26T00:42:45.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1201976371669454', N'1', CAST(N'2025-04-26T00:42:45.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1211061680650526', N'Rẻ quá shop', CAST(N'2025-04-26T00:42:34.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1216720220183169', N'1', CAST(N'2025-04-26T00:47:43.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1217644053116468', N'đâu', CAST(N'2025-04-26T00:41:07.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1247176753410415', N'BBM026 3', CAST(N'2025-04-26T00:49:22.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1331046121518638', N'BTL 002', CAST(N'2025-04-26T00:48:41.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1352522705794011', N'2', CAST(N'2025-04-26T00:45:14.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1398608764640213', N'Chào shop nha', CAST(N'2025-04-26T00:39:41.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1403810117418932', N'1', CAST(N'2025-04-26T00:43:02.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1446769206687535', N'BTL001 x3', CAST(N'2025-04-26T00:43:25.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1447625763354019', N'1', CAST(N'2025-04-26T00:42:44.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1606114636764482', N'BTL001', CAST(N'2025-04-26T00:48:45.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1622918305087357', N'1', CAST(N'2025-04-26T00:43:03.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1675347070527732', N'Có phải nhắn tin với page trước ko shop', CAST(N'2025-04-26T00:49:02.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1737000440568347', N'1', CAST(N'2025-04-26T00:43:02.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1752986985621686', N'Shop bán vì đam mê à', CAST(N'2025-04-26T00:42:48.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1764658397807923', N'2', CAST(N'2025-04-26T00:45:53.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1792885361308233', N'1', CAST(N'2025-04-26T00:41:57.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1811809809400050', N'nay shop bán gì thế', CAST(N'2025-04-26T00:40:10.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1825835214928623', N'tắt meet đi', CAST(N'2025-04-26T00:39:52.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_1899759234129117', N'1', CAST(N'2025-04-26T00:41:54.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_2124467664688992', N'lag the', CAST(N'2025-04-26T00:40:36.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_2159170004507945', N'Thật ko shop', CAST(N'2025-04-26T00:47:19.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_2306461303260441', N'1', CAST(N'2025-04-26T00:42:46.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_23892860036966271', N'Ông đừng gạch chân', CAST(N'2025-04-26T00:43:26.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_24029032213347066', N'1', CAST(N'2025-04-26T00:42:00.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_2465677153781828', N'Giá cả sao b ơi', CAST(N'2025-04-26T00:42:17.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_29832825076301277', N'tắt mic trong meet di', CAST(N'2025-04-26T00:39:39.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_3025693934265607', N'Thiên long', CAST(N'2025-04-26T00:42:43.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_3076461235860373', N'BTL 001', CAST(N'2025-04-26T00:43:10.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_3117297888411302', N'Hello shop', CAST(N'2025-04-26T00:39:02.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_4140846506237513', N'nay bán muộn thế', CAST(N'2025-04-26T00:39:22.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_523012837544117', N'chưa báo giá shop ơi', CAST(N'2025-04-26T00:42:04.0000000' AS DateTime2), 66)
GO
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_571522862099357', N'mình nhận dc order rồi sop oi', CAST(N'2025-04-26T00:44:53.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_594321782917759', N'Shop ghim đi shop', CAST(N'2025-04-26T00:40:02.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_600985872994839', N'Rẻ quá', CAST(N'2025-04-26T00:47:22.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_644052851869223', N'2', CAST(N'2025-04-26T00:45:14.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_648715181342123', N'Má ơi', CAST(N'2025-04-26T00:42:45.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_651957657612397', N'háo hức quá', CAST(N'2025-04-26T00:40:11.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_652071251144189', N'Shop lên nhanh lên mk săn còn đi ngủ', CAST(N'2025-04-26T00:46:30.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_666002852835550', N'Họ gạch chân mình ko lấy đx đâu', CAST(N'2025-04-26T00:43:37.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_670195795772976', N'2', CAST(N'2025-04-26T00:45:13.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_671654875830355', N'Shop k đi ngủ à', CAST(N'2025-04-26T00:39:10.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_678131625164406', N'BTL001 2', CAST(N'2025-04-26T00:43:10.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_679797544594650', N'1', CAST(N'2025-04-26T00:43:03.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_683177811352919', N'Đấy', CAST(N'2025-04-26T00:40:19.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_686042173911842', N'chào sốp', CAST(N'2025-04-26T00:39:15.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_691386523290163', N'mình nhận được order rồi nhé', CAST(N'2025-04-26T00:44:17.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_692667576586746', N'2', CAST(N'2025-04-26T00:45:15.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_703801058887453', N'Ône ổn rồi đấu', CAST(N'2025-04-26T00:40:16.0000000' AS DateTime2), 67)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_708586448264625', N'2', CAST(N'2025-04-26T00:45:13.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_709880504717218', N'háo hức quá sp tiếp theo là gì v', CAST(N'2025-04-26T00:45:57.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_712876441069877', N'1', CAST(N'2025-04-26T00:43:03.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_9498740973512208', N'sản hẩm này dùng được bao lâu', CAST(N'2025-04-26T00:41:32.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_9684038925022351', N'shop lên sản phẩm đi', CAST(N'2025-04-26T00:40:24.0000000' AS DateTime2), 65)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_9930661876998541', N'2', CAST(N'2025-04-26T00:45:15.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1737092183686750_995279722712904', N'1', CAST(N'2025-04-26T00:42:46.0000000' AS DateTime2), 66)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_1018013263224317', N'A002 3', CAST(N'2025-04-24T14:42:08.0000000' AS DateTime2), 14)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_1187609373093727', N'a003', CAST(N'2025-04-24T14:25:24.0000000' AS DateTime2), 17)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_1370100490977586', N'a003 20', CAST(N'2025-04-24T14:26:18.0000000' AS DateTime2), 17)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_1385274579171839', N'a003', CAST(N'2025-04-24T14:25:57.0000000' AS DateTime2), 17)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_1589409218392151', N'A002 3', CAST(N'2025-04-24T14:45:14.0000000' AS DateTime2), 14)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_3709677982671555', N'A001', CAST(N'2025-04-24T19:28:22.0000000' AS DateTime2), 22)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_3920927388218350', N'Bình luận', CAST(N'2025-04-24T14:39:16.0000000' AS DateTime2), 16)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_9498013730323946', N'A003', CAST(N'2025-04-24T14:26:06.0000000' AS DateTime2), 16)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_9537098203063947', N'a003 3', CAST(N'2025-04-24T14:26:03.0000000' AS DateTime2), 15)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1873357960081608_974404538065733', N'a003 2', CAST(N'2025-04-24T14:25:57.0000000' AS DateTime2), 14)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1009617824475904', N'g', CAST(N'2025-04-26T01:47:30.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1033218455398925', N'g', CAST(N'2025-04-26T01:47:31.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1046054627379837', N'23', CAST(N'2025-04-26T01:47:45.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1102426525268010', N'hhh', CAST(N'2025-04-26T01:47:27.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1178278663563867', N'S', CAST(N'2025-04-26T01:46:17.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1178447517090564', N'12', CAST(N'2025-04-26T01:46:13.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1179223630602854', N'11', CAST(N'2025-04-26T01:46:10.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1181717593141121', N'16', CAST(N'2025-04-26T01:46:35.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1187142766036766', N'18', CAST(N'2025-04-26T01:46:42.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1187221432880165', N'jsdhfdshf', CAST(N'2025-04-26T01:47:26.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1193206602475912', N'17', CAST(N'2025-04-26T01:46:38.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1193876732209087', N'g', CAST(N'2025-04-26T01:47:28.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1200652915123125', N'g', CAST(N'2025-04-26T01:47:30.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1212087480418836', N'5', CAST(N'2025-04-26T01:45:55.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1229881658504379', N'Ksnsnbs', CAST(N'2025-04-26T01:46:25.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1270683341347456', N'Babkka', CAST(N'2025-04-26T01:46:53.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1312201996546736', N'10', CAST(N'2025-04-26T01:46:06.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1332004781423102', N'g', CAST(N'2025-04-26T01:47:31.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1350340726176709', N'g', CAST(N'2025-04-26T01:47:30.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1350475126195882', N'Njsj', CAST(N'2025-04-26T01:46:23.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1352276156041045', N'14', CAST(N'2025-04-26T01:46:30.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1394788628531137', N'A', CAST(N'2025-04-26T01:46:15.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1403800663853043', N'g', CAST(N'2025-04-26T01:47:30.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1705610053386580', N'Ndj', CAST(N'2025-04-26T01:46:44.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1723640351837949', N'9', CAST(N'2025-04-26T01:46:03.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1735245507070713', N'Jdjs', CAST(N'2025-04-26T01:46:45.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1838165503415284', N'30', CAST(N'2025-04-26T01:47:11.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_1920741375422396', N'24', CAST(N'2025-04-26T01:47:50.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_2000832170442081', N'g', CAST(N'2025-04-26T01:47:31.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_2034082023746963', N'2', CAST(N'2025-04-26T01:45:47.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_2049719535535893', N'Nsbna', CAST(N'2025-04-26T01:46:51.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_2084239615412152', N'10', CAST(N'2025-04-26T01:47:16.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_2179054232544767', N'g', CAST(N'2025-04-26T01:47:29.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_2418251325201717', N'Bsjjs', CAST(N'2025-04-26T01:47:18.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_2690134787863217', N'g', CAST(N'2025-04-26T01:47:29.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_2735375723320359', N'g', CAST(N'2025-04-26T01:47:29.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_2839605739559611', N'1', CAST(N'2025-04-26T01:45:45.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_3025041827651569', N'25', CAST(N'2025-04-26T01:47:53.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_3980399028881143', N'g', CAST(N'2025-04-26T01:47:29.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_4050473831940694', N'Da', CAST(N'2025-04-26T01:46:21.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_545158141647073', N'40', CAST(N'2025-04-26T01:49:00.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_580128011148620', N'19', CAST(N'2025-04-26T01:47:06.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_580862054409072', N'7', CAST(N'2025-04-26T01:45:59.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_650474397786267', N'13', CAST(N'2025-04-26T01:46:28.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_654261420740468', N'15', CAST(N'2025-04-26T01:46:33.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_658870920405347', N'Nsjjs', CAST(N'2025-04-26T01:46:47.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_677016181404942', N'Sp', CAST(N'2025-04-26T01:46:19.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_683991291169153', N'8', CAST(N'2025-04-26T01:46:01.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_688529593678441', N'4', CAST(N'2025-04-26T01:45:52.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_691395189961088', N'Kakjsbs', CAST(N'2025-04-26T01:46:55.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_704757802223469', N'3', CAST(N'2025-04-26T01:45:49.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_707444785302277', N'g', CAST(N'2025-04-26T01:47:31.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_713085738056309', N'20', CAST(N'2025-04-26T01:47:08.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_978209507483186', N'6', CAST(N'2025-04-26T01:45:57.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_981070674195688', N'Jsjs', CAST(N'2025-04-26T01:46:49.0000000' AS DateTime2), 69)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_987176180208531', N'g', CAST(N'2025-04-26T01:47:29.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_989899182883373', N'g', CAST(N'2025-04-26T01:47:28.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'1889219425248749_996962435978453', N'g', CAST(N'2025-04-26T01:47:30.0000000' AS DateTime2), 70)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1046736477363220', N'AT001 2', CAST(N'2025-04-19T17:03:17.0000000' AS DateTime2), 2)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1173858480632484', N'ffd', CAST(N'2025-04-16T13:27:14.0000000' AS DateTime2), 3)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1218069969534660', N'ABC123 1', CAST(N'2025-04-20T22:15:47.0000000' AS DateTime2), 2)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1241336834139858', N'ABC123 4', CAST(N'2025-04-20T22:16:04.0000000' AS DateTime2), 2)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1342407896879078', N'678', CAST(N'2025-04-16T13:27:20.0000000' AS DateTime2), 3)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1348458106430730', N'123', CAST(N'2025-04-16T13:27:15.0000000' AS DateTime2), 3)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1351313036096134', N'ABC123 3', CAST(N'2025-04-20T21:52:13.0000000' AS DateTime2), 2)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1352973672691277', N'ATT001 2', CAST(N'2025-04-20T21:48:09.0000000' AS DateTime2), 2)
GO
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1356578402317390', N'hihihihhih', CAST(N'2025-04-16T12:44:18.0000000' AS DateTime2), 5)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1372833293866817', N'Pdt 1', CAST(N'2025-04-16T13:27:30.0000000' AS DateTime2), 4)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1457284835607573', N'Pdt1', CAST(N'2025-04-16T14:26:59.0000000' AS DateTime2), 4)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1624163114883057', N'Pdt1 x2', CAST(N'2025-04-16T13:28:00.0000000' AS DateTime2), 4)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1682967835923060', N'Pdt1', CAST(N'2025-04-16T13:27:37.0000000' AS DateTime2), 4)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1811705609673005', N'234', CAST(N'2025-04-16T13:27:19.0000000' AS DateTime2), 3)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_1985407278535624', N'ATT001 45', CAST(N'2025-04-19T22:54:44.0000000' AS DateTime2), 2)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_3676485685978095', N'ABC123 2', CAST(N'2025-04-20T22:10:26.0000000' AS DateTime2), 2)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_458896533936440', N'Abc 123', CAST(N'2025-04-20T22:16:26.0000000' AS DateTime2), 4)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_601281829741142', N'P001', CAST(N'2025-04-22T21:34:44.0000000' AS DateTime2), 3)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_606733272385355', N'ABC124', CAST(N'2025-04-20T22:16:20.0000000' AS DateTime2), 4)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_687987206954616', N'ABC123 3', CAST(N'2025-04-20T22:15:58.0000000' AS DateTime2), 2)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_690208503423125', N'Pdt1 2', CAST(N'2025-04-16T13:27:49.0000000' AS DateTime2), 4)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_718559953829106', N'ABC123 3', CAST(N'2025-04-20T22:16:09.0000000' AS DateTime2), 2)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2013529655806022_896498852544799', N'345', CAST(N'2025-04-16T13:27:17.0000000' AS DateTime2), 3)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2191324188004453_540757695740464', N'B111', CAST(N'2025-04-25T21:06:18.0000000' AS DateTime2), 49)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2396079117439198_1227209255777183', N'P002', CAST(N'2025-04-24T09:56:39.0000000' AS DateTime2), 10)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2396079117439198_1247822623354385', N'A001', CAST(N'2025-04-24T19:35:30.0000000' AS DateTime2), 9)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2396079117439198_1497891231184883', N'A002', CAST(N'2025-04-24T19:36:33.0000000' AS DateTime2), 9)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2396079117439198_2015798829191234', N'P002', CAST(N'2025-04-24T09:56:00.0000000' AS DateTime2), 9)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2396079117439198_693052129840462', N'P001', CAST(N'2025-04-24T09:53:20.0000000' AS DateTime2), 9)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2396079117439198_9581978611893273', N'P0022', CAST(N'2025-04-24T09:56:15.0000000' AS DateTime2), 9)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1002851312033723', N'chao shop a', CAST(N'2025-04-28T16:00:58.0000000' AS DateTime2), 1078)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1011363821138744', N'ODT300 1', CAST(N'2025-04-28T16:04:34.0000000' AS DateTime2), 1075)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1040477091392798', N'Usb120 90', CAST(N'2025-04-28T16:06:11.0000000' AS DateTime2), 1080)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1044402794273469', N'shop có ốp ip14 k shop', CAST(N'2025-04-28T16:06:15.0000000' AS DateTime2), 1077)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1051641193529844', N'giá sao shop', CAST(N'2025-04-28T16:05:34.0000000' AS DateTime2), 1076)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1091338696145365', N'Chúc Shop buổi chiều vui vẻ', CAST(N'2025-04-28T16:00:33.0000000' AS DateTime2), 1076)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1153902059832998', N'BL700 1000', CAST(N'2025-04-28T16:06:35.0000000' AS DateTime2), 1080)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1162796522265820', N'Thế chốt BL700 13', CAST(N'2025-04-28T16:07:54.0000000' AS DateTime2), 1080)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1196783565189839', N'BL700 13', CAST(N'2025-04-28T16:08:14.0000000' AS DateTime2), 1080)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1211804773689953', N'usb120 44', CAST(N'2025-04-28T16:08:04.0000000' AS DateTime2), 1078)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1226794412498279', N'nói lại mã sp được ko ạ', CAST(N'2025-04-28T16:02:59.0000000' AS DateTime2), 1075)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1241677204206205', N'Chào Shop nha', CAST(N'2025-04-28T16:00:16.0000000' AS DateTime2), 1076)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1314244070038237', N'DH400 1', CAST(N'2025-04-28T16:04:07.0000000' AS DateTime2), 1077)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1330924674685858', N'💬 Comment theo cú pháp:
 👉 Mã sản phẩm + số lượng
 🔖 Ví dụ: A001 2 (mua 2 sản phẩm mã A001)', CAST(N'2025-04-28T16:03:33.0000000' AS DateTime2), 1079)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1363470451366571', N'Odt300', CAST(N'2025-04-28T16:04:30.0000000' AS DateTime2), 1076)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1620793521912935', N'bạn lên kìm bấm móng tay đi ạ, đúng thứ mình đang cần', CAST(N'2025-04-28T16:08:57.0000000' AS DateTime2), 1076)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1754144288787401', N'shop cho minh xin mã đồng hồ với', CAST(N'2025-04-28T16:03:20.0000000' AS DateTime2), 1078)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1839066280210829', N'Hello shop', CAST(N'2025-04-28T16:00:26.0000000' AS DateTime2), 1077)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_1953602582078877', N'mã ốp điện thoại', CAST(N'2025-04-28T16:03:43.0000000' AS DateTime2), 1075)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_2050429965466939', N'DH400 3', CAST(N'2025-04-28T16:04:01.0000000' AS DateTime2), 1080)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_2145452265970967', N'Thôi, mình phải chạy rồi. Gộp đơn cho mình rồi hd mình ck nhé', CAST(N'2025-04-28T16:07:18.0000000' AS DateTime2), 1080)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_2178573409255672', N'nhận hàng được kiểm tra k shop?', CAST(N'2025-04-28T16:06:16.0000000' AS DateTime2), 1078)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_29215214801459956', N'đồng hồ giá như nào ạ', CAST(N'2025-04-28T16:01:46.0000000' AS DateTime2), 1077)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_3136588586498917', N'BL700 20', CAST(N'2025-04-28T16:06:37.0000000' AS DateTime2), 1076)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_3934100280138184', N'ốp đi shop', CAST(N'2025-04-28T16:03:43.0000000' AS DateTime2), 1077)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_4021231548122696', N'mã commet là gì thế shop', CAST(N'2025-04-28T16:02:03.0000000' AS DateTime2), 1077)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_546637511826865', N'usb120 1', CAST(N'2025-04-28T16:06:22.0000000' AS DateTime2), 1075)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_557438203655988', N'chào shop nha', CAST(N'2025-04-28T16:00:12.0000000' AS DateTime2), 1075)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_572379825891896', N'Odt300 1', CAST(N'2025-04-28T16:04:52.0000000' AS DateTime2), 1078)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_700373689185011', N'mã như nào shop', CAST(N'2025-04-28T16:03:11.0000000' AS DateTime2), 1077)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'2557515231281729_711769907965333', N'quá hời', CAST(N'2025-04-28T16:05:45.0000000' AS DateTime2), 1076)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3560107860961290_1060128346178254', N'a001', CAST(N'2025-04-25T16:39:28.0000000' AS DateTime2), 40)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3560107860961290_533321829622023', N'a001', CAST(N'2025-04-25T16:44:09.0000000' AS DateTime2), 42)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3560107860961290_650616971102336', N'a001', CAST(N'2025-04-25T16:44:41.0000000' AS DateTime2), 42)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3560107860961290_661577796614684', N'a001', CAST(N'2025-04-25T16:42:20.0000000' AS DateTime2), 41)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3560107860961290_710369488320729', N'a001', CAST(N'2025-04-25T16:44:48.0000000' AS DateTime2), 42)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3932880747040476_1220484179440694', N'Pdt3', CAST(N'2025-04-25T14:30:12.0000000' AS DateTime2), 37)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3932880747040476_1314669879832609', N'Pdt1', CAST(N'2025-04-25T14:30:04.0000000' AS DateTime2), 37)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3932880747040476_2402920936737689', N'Pdt1', CAST(N'2025-04-25T14:33:26.0000000' AS DateTime2), 37)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3932880747040476_577810955327717', N'Pdt 1', CAST(N'2025-04-25T14:30:01.0000000' AS DateTime2), 37)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3932880747040476_673208575307955', N'Pdt1', CAST(N'2025-04-25T14:43:03.0000000' AS DateTime2), 37)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3932880747040476_676659161679346', N'Pdt', CAST(N'2025-04-25T14:36:31.0000000' AS DateTime2), 37)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'3932880747040476_700694319183489', N'Pdt1', CAST(N'2025-04-25T14:36:35.0000000' AS DateTime2), 37)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'486374971131519_1427968721779512', N'B111', CAST(N'2025-04-25T21:16:01.0000000' AS DateTime2), 50)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'643745255200836_1091813109637379', N'A003', CAST(N'2025-04-24T21:44:06.0000000' AS DateTime2), 30)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'643745255200836_1206381796996457', N'A003 x số lượng', CAST(N'2025-04-24T21:43:04.0000000' AS DateTime2), 29)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'643745255200836_1220837659770164', N'Helo', CAST(N'2025-04-24T21:43:23.0000000' AS DateTime2), 30)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'643745255200836_2410549852645424', N'Shop', CAST(N'2025-04-24T21:43:33.0000000' AS DateTime2), 30)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'643745255200836_2827505394086344', N'A003', CAST(N'2025-04-24T21:44:19.0000000' AS DateTime2), 30)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'643745255200836_559555600498023', N'Chào ship', CAST(N'2025-04-24T21:43:30.0000000' AS DateTime2), 30)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'643745255200836_971282961878321', N'A003 x2', CAST(N'2025-04-24T21:44:01.0000000' AS DateTime2), 30)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1022002473192760', N'A003234', CAST(N'2025-04-24T21:25:48.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1025714339622367', N'A003', CAST(N'2025-04-24T21:24:50.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1067064945271549', N'Tùng k cmt à', CAST(N'2025-04-24T21:24:38.0000000' AS DateTime2), 25)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1201598698209298', N'A003 x2', CAST(N'2025-04-24T21:25:04.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1238199551206623', N'Có 3 cái t mua 4 cái rồi', CAST(N'2025-04-24T21:25:34.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1334863791145886', N'Vãi', CAST(N'2025-04-24T21:24:17.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1412072566639887', N'Tại sao lại nhiều cmt thế', CAST(N'2025-04-24T21:21:20.0000000' AS DateTime2), 25)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1453845095793625', N'Tùng đâu', CAST(N'2025-04-24T21:23:21.0000000' AS DateTime2), 25)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1455987052037574', N'Xem đx ko e', CAST(N'2025-04-24T21:24:38.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1531369091107311', N'Ghim cmt', CAST(N'2025-04-24T21:22:31.0000000' AS DateTime2), 25)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1632300984082407', N'Thật k vậy shop', CAST(N'2025-04-24T21:23:03.0000000' AS DateTime2), 25)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1843100523116251', N'Tắt đi vào cái khách đi mạnh', CAST(N'2025-04-24T21:26:18.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_1887866081986022', N'A003 x số lượng để chốt đơn', CAST(N'2025-04-24T21:22:40.0000000' AS DateTime2), 26)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_2077473559400768', N'gim đi', CAST(N'2025-04-24T21:24:32.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_456550437516777', N'A003 20', CAST(N'2025-04-24T21:25:40.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_555514947593433', N'A003', CAST(N'2025-04-24T21:24:45.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_560353370505264', N'Mẫu này tốt k', CAST(N'2025-04-24T21:20:40.0000000' AS DateTime2), 25)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_584764200629630', N'Vừa vào nhần paghe', CAST(N'2025-04-24T21:24:25.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_606492775779057', N'A003 x số lượng để chốt', CAST(N'2025-04-24T21:24:17.0000000' AS DateTime2), 27)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_671412942141059', N'A003 3', CAST(N'2025-04-24T21:26:09.0000000' AS DateTime2), 25)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_952333100308526', N'Mạnh ơi mở live khác đi', CAST(N'2025-04-24T21:27:05.0000000' AS DateTime2), 28)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_995022079050805', N'Mua nhiều cái xem nào', CAST(N'2025-04-24T21:25:06.0000000' AS DateTime2), 25)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'660697383556750_998686429035358', N'Cmt đi', CAST(N'2025-04-24T21:23:27.0000000' AS DateTime2), 25)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_1175568247215443', N'A001', CAST(N'2025-04-25T16:43:31.0000000' AS DateTime2), 38)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_1221862166228031', N'a001 cho mình 5 đơn nhé shop', CAST(N'2025-04-25T16:35:01.0000000' AS DateTime2), 38)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_1341286697086122', N'hello các bạn', CAST(N'2025-04-25T16:29:21.0000000' AS DateTime2), 38)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_1353000822657092', N'a001', CAST(N'2025-04-25T16:30:44.0000000' AS DateTime2), 38)
GO
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_1401893447919560', N'a001 3', CAST(N'2025-04-25T16:35:59.0000000' AS DateTime2), 38)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_1641209846531069', N'comment ghim ở đâu đấy', CAST(N'2025-04-25T16:29:31.0000000' AS DateTime2), 38)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_1696116974628879', N'a002', CAST(N'2025-04-25T16:30:23.0000000' AS DateTime2), 38)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_2440979669569128', N'A001', CAST(N'2025-04-25T16:43:10.0000000' AS DateTime2), 38)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_643097962038046', N'a001', CAST(N'2025-04-25T16:30:59.0000000' AS DateTime2), 39)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'663441859726132_653711844175779', N'a001', CAST(N'2025-04-25T16:43:36.0000000' AS DateTime2), 38)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1020457486258207', N'4', CAST(N'2025-04-25T23:38:28.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1034681741343854', N'ko phải bill', CAST(N'2025-04-25T23:35:18.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1043734451140739', N'Shopp ơi', CAST(N'2025-04-25T23:45:14.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1050546440298499', N'Chào shop nha', CAST(N'2025-04-26T00:39:24.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1067644108534199', N'giới thiệu giá', CAST(N'2025-04-25T23:36:37.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1115211193697314', N'giá sale còn 4 k', CAST(N'2025-04-25T23:37:06.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1184487980139559', N'4', CAST(N'2025-04-25T23:38:33.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1187922099102389', N'ai muốn mua với giá 4k comment số 4', CAST(N'2025-04-25T23:37:16.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1192241465646518', N'444', CAST(N'2025-04-25T23:38:32.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1210467567297160', N'giá đâu 30Shine Shop', CAST(N'2025-04-25T23:36:46.0000000' AS DateTime2), 57)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1223145799398200', N'B111 5', CAST(N'2025-04-25T23:38:43.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1260020549458877', N'💬 Comment theo cú pháp:
👉 Mã sản phẩm + số lượng
🔖 Ví dụ: A001 x2 (mua 2 sản phẩm mã A001)


📩 Sau khi comment, nhớ NHẮN TIN TRƯỚC cho fanpage để được chốt đơn nhanh và giữ hàng nha cả nhà ơi!', CAST(N'2025-04-25T23:33:00.0000000' AS DateTime2), 55)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1320231185936688', N'lên mã sp roi thi dung bat cmt so 4 nua', CAST(N'2025-04-25T23:39:15.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1368680364280807', N'gap bug roi sop', CAST(N'2025-04-25T23:42:27.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1454419848774457', N'nay có sản phẩm gì thế', CAST(N'2025-04-25T23:34:57.0000000' AS DateTime2), 57)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1471838764188649', N'4', CAST(N'2025-04-25T23:38:30.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1476651813314686', N'4', CAST(N'2025-04-25T23:38:31.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1634199357222486', N'4', CAST(N'2025-04-25T23:38:30.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1717426082179136', N'helo', CAST(N'2025-04-25T23:41:21.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1718097282119625', N'A003 x5', CAST(N'2025-04-25T23:59:29.0000000' AS DateTime2), 55)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1731587780763986', N'4', CAST(N'2025-04-25T23:38:31.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1750642435524148', N'wow tuyệt vời', CAST(N'2025-04-25T23:37:42.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1813355166126217', N'demo chức năng bút đã', CAST(N'2025-04-25T23:36:53.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1821690185349492', N'mai có in mã qr ra cho đẹp nữa', CAST(N'2025-04-25T23:35:58.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1837433840429684', N'4', CAST(N'2025-04-25T23:38:29.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1844609666394006', N'nét bút cũng đẹp đấy', CAST(N'2025-04-25T23:37:53.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1862301594533736', N'4', CAST(N'2025-04-25T23:38:31.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_1940756209792845', N'4', CAST(N'2025-04-25T23:38:32.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_2061748241014997', N'chào shop nha', CAST(N'2025-04-25T23:34:16.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_2087682205077465', N'4', CAST(N'2025-04-25T23:38:10.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_24106692295600520', N'shop  nay bán gì vậy', CAST(N'2025-04-25T23:34:49.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_2753189385072338', N'4', CAST(N'2025-04-25T23:38:29.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_4281934328792007', N'tu tu đã', CAST(N'2025-04-25T23:42:36.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_471351319335062', N'hello', CAST(N'2025-04-25T23:41:39.0000000' AS DateTime2), 57)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_554683347267107', N'ok day', CAST(N'2025-04-25T23:40:52.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_585334660572005', N'4', CAST(N'2025-04-25T23:38:29.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_587069707043003', N'bình tĩnh thoi sop', CAST(N'2025-04-25T23:41:49.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_600094963045907', N'k có giá à shop', CAST(N'2025-04-25T23:37:03.0000000' AS DateTime2), 57)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_608529035550957', N'lên sản phẩm', CAST(N'2025-04-25T23:35:14.0000000' AS DateTime2), 58)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_654220064059374', N'4', CAST(N'2025-04-25T23:38:31.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_695698792838162', N'giới thiệu với giá nữa', CAST(N'2025-04-25T23:36:55.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_697946892794277', N'4', CAST(N'2025-04-25T23:38:31.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_698991226127178', N'4', CAST(N'2025-04-25T23:38:30.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_701213289084221', N'B111 x2', CAST(N'2025-04-25T23:38:42.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_707671608275848', N'ok shop ơi', CAST(N'2025-04-25T23:36:07.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'677180408277938_730824819288127', N'44', CAST(N'2025-04-25T23:38:32.0000000' AS DateTime2), 56)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'686305647225688_1471421810336465', N'a001 x3', CAST(N'2025-04-25T16:57:59.0000000' AS DateTime2), 46)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'700344615888479_1417007176330456', N'A001', CAST(N'2025-04-25T17:09:13.0000000' AS DateTime2), 47)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'700344615888479_1726355574982829', N'A00', CAST(N'2025-04-25T17:06:21.0000000' AS DateTime2), 47)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1013753290733010', N'sa200 1000', CAST(N'2025-04-26T22:23:02.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1021983049460371', N'Vở+bút+tẩy', CAST(N'2025-04-26T22:22:36.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1026746329518450', N'chao bac a', CAST(N'2025-04-26T22:14:03.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1037289018448637', N'mình muốn chốt', CAST(N'2025-04-26T22:20:00.0000000' AS DateTime2), 82)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1037977128214834', N'BTL150 2', CAST(N'2025-04-26T22:15:23.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1039729364694741', N'minh vừa cho con học bài nào của vở là bnhieu thế', CAST(N'2025-04-26T22:35:32.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1091609619677513', N'SA200 5', CAST(N'2025-04-26T22:31:05.0000000' AS DateTime2), 85)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1185898902674661', N'chuyển khoản như nào thế shop', CAST(N'2025-04-26T22:16:22.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1195444045626176', N'tẩy dùng dc bao lâu thế shop', CAST(N'2025-04-26T22:20:20.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1207999597395276', N'BTL150 1', CAST(N'2025-04-26T22:17:14.0000000' AS DateTime2), 84)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1212117460628509', N'cho minh mã đơn hàng mới', CAST(N'2025-04-26T22:15:15.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1217213930077158', N'hay qua', CAST(N'2025-04-26T22:15:06.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1222834872895804', N'SA200 3', CAST(N'2025-04-26T22:28:00.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1228970251922738', N'💬 Comment theo cú pháp:
 👉 Mã sản phẩm + số lượng
 🔖 Ví dụ: A001 x2 (mua 2 sản phẩm mã A001)
📩 Sau khi comment, nhớ NHẮN TIN TRƯỚC cho fanpage để được chốt đơn nhanh và giữ hàng nha cả nhà ơi!', CAST(N'2025-04-26T22:12:54.0000000' AS DateTime2), 79)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1289125322821938', N'Tay200 1', CAST(N'2025-04-26T22:20:08.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1318347309464943', N'mình k thấy sản phẩm', CAST(N'2025-04-26T22:15:37.0000000' AS DateTime2), 82)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1348975586409292', N'toi muốn xem vở đc k shop', CAST(N'2025-04-26T22:17:21.0000000' AS DateTime2), 82)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1351255422764424', N'Hello shop', CAST(N'2025-04-26T22:13:31.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1361482971827554', N'Cho mình xem saccsh Klong vs', CAST(N'2025-04-26T22:22:08.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1366489694389612', N'SA200 1000000000000', CAST(N'2025-04-26T22:27:27.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1373779043935385', N'tôi muốn xem tẩy', CAST(N'2025-04-26T22:19:06.0000000' AS DateTime2), 82)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1392518348835320', N'mã sp của bút là gì thế mình muốn đặt Ghế massage cao cấp chính hãng Hakita Thái Bình', CAST(N'2025-04-26T22:20:08.0000000' AS DateTime2), 84)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1393674088622323', N'A001x2', CAST(N'2025-04-26T22:33:38.0000000' AS DateTime2), 86)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1416706359671522', N'SA200 -10000', CAST(N'2025-04-26T22:23:44.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1429643275065398', N'BTL150 3', CAST(N'2025-04-26T22:16:12.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1435499964283628', N'nay shop có những gì thế', CAST(N'2025-04-26T22:14:15.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1700666477250298', N'cho cam xa ra được k shop', CAST(N'2025-04-26T22:15:28.0000000' AS DateTime2), 82)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1770920776827544', N'sao mk đặt 1000 cuốn k đc', CAST(N'2025-04-26T22:28:41.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1790336371896821', N'T500 1', CAST(N'2025-04-26T22:20:22.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_1951899275217834', N'mã là gì vậy ạ', CAST(N'2025-04-26T22:19:56.0000000' AS DateTime2), 82)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_2064559494053302', N'Sa200 1000', CAST(N'2025-04-26T22:23:26.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_2088068548283605', N'Có bán combo kiểu bia kèm lạc không?', CAST(N'2025-04-26T22:22:20.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_2119052491900400', N't200 5', CAST(N'2025-04-26T22:20:14.0000000' AS DateTime2), 82)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_2135297616893118', N'mình đã đặt hàng thành công ạ', CAST(N'2025-04-26T22:16:29.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_2729621257237729', N'Cho mình xin giá vở với', CAST(N'2025-04-26T22:33:10.0000000' AS DateTime2), 86)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_2787564384778699', N'bao giờ  đến mã  của vở thế bạn', CAST(N'2025-04-26T22:21:11.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_3714074948736427', N'mình muốn cho đồ cho bé nhà mình để đi học', CAST(N'2025-04-26T22:20:38.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_4011428605811806', N'hộp thay đầu bút mã bao nhiêu thế bạn', CAST(N'2025-04-26T22:25:43.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_4076102379379747', N'Ab200', CAST(N'2025-04-26T22:23:16.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_4076438972589631', N'Mình spam đơn 1 loại sp thì có gộp đơn không?', CAST(N'2025-04-26T22:25:06.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_4873144752911523', N'sA200 -1000', CAST(N'2025-04-26T22:23:36.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_557924820263017', N'T201 1', CAST(N'2025-04-26T22:20:16.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_603840159352278', N'SA200 10000', CAST(N'2025-04-26T22:23:52.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_643329105350288', N'Thiếu thông tin tk để chuyển cọc rồi', CAST(N'2025-04-26T22:15:55.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_650682074526826', N'BTL150  10', CAST(N'2025-04-26T22:15:43.0000000' AS DateTime2), 81)
GO
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_665078762985101', N'Nhắn tin gì?', CAST(N'2025-04-26T22:17:33.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_668525149145890', N'1', CAST(N'2025-04-26T22:14:08.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_669257969034533', N'cho cam gần quá shop', CAST(N'2025-04-26T22:15:11.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_673831158572344', N'bút có mấy loại v sop', CAST(N'2025-04-26T22:22:00.0000000' AS DateTime2), 84)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_686183560453837', N'Hi shop', CAST(N'2025-04-26T22:32:57.0000000' AS DateTime2), 86)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_688161440372829', N'AS200 1', CAST(N'2025-04-26T22:23:03.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_691057836741642', N'hnay shop cua minh sale cgi the', CAST(N'2025-04-26T22:14:40.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_698900932599251', N'mình muốn xem vở được không ạ', CAST(N'2025-04-26T22:19:45.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_703191508801312', N'shop sale muon the', CAST(N'2025-04-26T22:14:20.0000000' AS DateTime2), 82)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_706193002082264', N't200 3', CAST(N'2025-04-26T22:20:25.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_709273738131477', N'SA 200 5', CAST(N'2025-04-26T22:23:18.0000000' AS DateTime2), 85)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_716740817377134', N'SA200 10', CAST(N'2025-04-26T22:34:49.0000000' AS DateTime2), 86)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_720131937123021', N'BTL150 4', CAST(N'2025-04-26T22:16:42.0000000' AS DateTime2), 81)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_9598417526911896', N'T200 3', CAST(N'2025-04-26T22:20:21.0000000' AS DateTime2), 84)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_972344364718414', N'sách còn bao cuốn thế', CAST(N'2025-04-26T22:28:34.0000000' AS DateTime2), 80)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9358466337612802_996093859356991', N'T200 5', CAST(N'2025-04-26T22:20:32.0000000' AS DateTime2), 83)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_1218489769618281', N'002', CAST(N'2025-04-22T17:57:12.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_1395874311836596', N'P002', CAST(N'2025-04-22T21:10:37.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_1775119436367844', N'P003', CAST(N'2025-04-22T21:10:47.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_1859104341531866', N'P001', CAST(N'2025-04-22T21:10:50.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_2230972627299426', N'002', CAST(N'2025-04-22T17:57:16.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_2649294551947017', N'p003', CAST(N'2025-04-22T21:10:39.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_3924541844432856', N'001', CAST(N'2025-04-22T17:57:13.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_484114084695740', N'P002', CAST(N'2025-04-22T21:09:27.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_530981143193787', N'001', CAST(N'2025-04-22T17:57:09.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_601176503085015', N'P002', CAST(N'2025-04-22T21:10:53.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_692023006691356', N'P003', CAST(N'2025-04-22T21:11:00.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'981287650659327_902723195255672', N'003', CAST(N'2025-04-22T17:57:18.0000000' AS DateTime2), 1)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_1232655095231621', N'Mình muốn mua', CAST(N'2025-04-24T14:42:02.0000000' AS DateTime2), 18)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_1241718590849473', N'Máy in bằng gì vậy ạ', CAST(N'2025-04-24T14:45:22.0000000' AS DateTime2), 18)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_1299641641132372', N'a002 3', CAST(N'2025-04-24T14:45:41.0000000' AS DateTime2), 19)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_1304892867287064', N'a002 2', CAST(N'2025-04-24T14:41:59.0000000' AS DateTime2), 19)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_1308057650256590', N'a002 1', CAST(N'2025-04-24T14:45:50.0000000' AS DateTime2), 21)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_1673854349935099', N'Hay quá', CAST(N'2025-04-24T14:41:44.0000000' AS DateTime2), 18)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_1678270206149366', N'mã bao nhiêu nhiêu, giá bao nhiêu?', CAST(N'2025-04-24T14:46:01.0000000' AS DateTime2), 20)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_1733535010565890', N'a002 1', CAST(N'2025-04-24T14:46:13.0000000' AS DateTime2), 20)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_2172359493201824', N'a002 2', CAST(N'2025-04-24T14:46:53.0000000' AS DateTime2), 20)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_568504942944905', N'sản phẩm gì thế', CAST(N'2025-04-24T14:45:48.0000000' AS DateTime2), 20)
INSERT [dbo].[Comments] ([CommentID], [Content], [CommentTime], [LiveStreamCustomerID]) VALUES (N'9960146630714785_704546722253002', N'A001', CAST(N'2025-04-24T19:31:56.0000000' AS DateTime2), 23)
GO
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'10096417470402179', N'Nguyen Cao Cuong', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXGFcICk6zSobIGR2o9IGjvbWbciMoiNzzORtA6mWopXpAu6IL1vrETkGoBqU1Qpe_u5msxJemL-&psid=10096417470402179&height=50&width=50&ext=1748165464&hash=AbaxEzZEC0ry0KGu7MsDuxND', N'', N'', N'', N'', 0, 0, CAST(N'2025-04-25T16:31:04.0278082' AS DateTime2), CAST(N'2025-04-25T16:31:04.0278088' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'266349363239226', N'EE', N'https://scontent.fhan2-4.fna.fbcdn.net/v/t39.30808-1/444468395_122104826606331196_4349240571494043555_n.jpg?stp=c151.151.698.698a_cp0_dst-jpg_s50x50_tt6&_nc_cat=105&ccb=1-7&_nc_sid=fe756c&_nc_eui2=AeFpOOyft16Z5bq2VcsvHFZwO5D35cydwsg7kPflzJ3CyPyMKJshgXBZFJ6GPGvVMos1jSLmNksqhBH3NfSwOeiI&_nc_ohc=KCv9yP25Gs8Q7kNvwEB7Acj&_nc_oc=AdkJF5jyRN1Oj4Dkey2uw45huERzyNYitldlasxPwZy12zUjVkeJ0F4DKZ2c1--QWW8&_nc_zt=24&_nc_ht=scontent.fhan2-4.fna&edm=AJ8zDikEAAAA&_nc_gid=417o6rjdAkAULIz3U0lX6Q&oh=00_AfH8J8vfU8MTuxEb3MGLxJgi8btISfP7QcOzT0TqVLolqQ&oe=680E8CD2', NULL, NULL, NULL, NULL, 0, 0, CAST(N'2025-04-23T15:22:08.6732834' AS DateTime2), CAST(N'2025-04-23T15:22:08.6732841' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'28115740348070899', N'Nguyễn Viết Lương', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXGNvnWbQ0rZ7aKspR0RJsLosfPNPVpCxNOH_p_aFG5FXu1xEeAk2qTCa8RHdtWIRG2IPWEBHjJv&psid=28115740348070899&height=50&width=50&ext=1748071260&hash=AbY87AC-dQsQtQ8Edu-N5qVk', NULL, NULL, NULL, NULL, 0, 0, CAST(N'2025-04-24T14:21:00.1576083' AS DateTime2), CAST(N'2025-04-24T14:21:00.1576098' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'28407649178881320', N'Hieu Pham', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXGiWmzxXe_H_hhk3BoPgYoJQmyOuG_sZAaSsVo1h_KWnFvLOlqtBmuNbPiOLlrBSvQF4EkXOIPz&psid=28407649178881320&height=50&width=50&ext=1747924497&hash=AbYBf-0flbCql4AImacj4LRs', NULL, NULL, NULL, NULL, 0, 0, CAST(N'2025-04-22T21:34:56.3407577' AS DateTime2), CAST(N'2025-04-22T21:34:56.3407597' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'7552312751525534', N'Thanh Tùng', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXGUwt3vs5O8WzK9wHbLdlw2QWYv-Xg9V8HxdLn_8B-F9qjRi_-nf-425-FlI3DrNl7CdsPmLucz&psid=7552312751525534&height=50&width=50&ext=1747924497&hash=AbY0W4-1-U-xg82dKI62fy2e', NULL, NULL, NULL, NULL, 0, 0, CAST(N'2025-04-22T21:34:56.5279423' AS DateTime2), CAST(N'2025-04-22T21:34:56.5279444' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'9170304843076058', N'Cầu Phạm', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXFclr0PL_2-vOesXFZphDe9ZAznH7UzubXOk_Hrvi2Py-O66MaJ_VPRC6DcovrP5uQkWEQ0rcRh&psid=9170304843076058&height=50&width=50&ext=1748101243&hash=Abb8iKCBxsgtEDBdZlHLH5kz', NULL, NULL, NULL, NULL, 0, 0, CAST(N'2025-04-24T22:40:43.1385446' AS DateTime2), CAST(N'2025-04-24T22:40:43.1385506' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'9239312666134797', N'Dũng Anh Hà', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXHPzRNzC14rx8I5RwJVRIsFkOcj_dlwCUKn8Y9CMlelu-wqvAORsRLIwc-L6ZTz2p1P83cRitXE&psid=9239312666134797&height=50&width=50&ext=1747924497&hash=AbbDRUck9NWPO4rHC2YOEdrO', NULL, NULL, NULL, NULL, 0, 0, CAST(N'2025-04-22T21:34:56.6284604' AS DateTime2), CAST(N'2025-04-22T21:34:56.6284625' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'9404298646256687', N'Mạnhh', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXEkqzr1oK3Kgrurp24Fsq-iSYcO_MXkYVDzLl6832a0nnPZ8mS158bFcyCq9O1J50vLSAVXqFsq&psid=9404298646256687&height=50&width=50&ext=1747911791&hash=AbZw4CgV6V3hB4m44Di7F7vr', N'manhga', N'3131346797', N'manh@gmail.com', N'hanoi', 0, 0, CAST(N'2025-04-22T18:03:11.1639281' AS DateTime2), CAST(N'2025-04-22T18:03:11.1639303' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'9535928183191974', N'Phạm Trang', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXElKm2vWER-AMEtJlU7XV9XXx5e83ibyTWfkhGRxqc33UJ0r7HPFvWt8BZ_UbtPjnNvgLdrb8C7&psid=9535928183191974&height=50&width=50&ext=1748166990&hash=Aba96BkblkivY7Yw8fEhncZW', NULL, NULL, NULL, NULL, 0, 0, CAST(N'2025-04-25T16:56:30.8310367' AS DateTime2), CAST(N'2025-04-25T16:56:30.8310375' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'9570129896356691', N'Phương Hoa', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXHq7ROy_27EQ-hhyYrF699htcv1MUTfYMyx8kjVaN4wsUkDMs5BEwIjKW3zV5-T_SMVeH4_ExOT&psid=9570129896356691&height=50&width=50&ext=1748167613&hash=AbZaz-Xiw-qbEXaPRCK8E2Lb', NULL, NULL, NULL, NULL, 0, 0, CAST(N'2025-04-25T17:06:52.9145780' AS DateTime2), CAST(N'2025-04-25T17:06:52.9145797' AS DateTime2), 0)
INSERT [dbo].[Customers] ([CustomerID], [FacebookName], [ImageURL], [FullName], [PhoneNumber], [Email], [Address], [SuccessfulDeliveries], [FailedDeliveries], [CreatedAt], [UpdatedAt], [Status]) VALUES (N'9849276838426644', N'Bui Ngoc Anh', N'https://platform-lookaside.fbsbx.com/platform/profilepic/?eai=AXE7_mutoBe588-uI1sbJDi8lFkHLlqNEggA2aU0nBgbncBjubPTt-0VE-Z6Mm-Sock3G27ohm7A&psid=9849276838426644&height=50&width=50&ext=1748072158&hash=Aba44Rql7UiPqNLaRPPh-FdV', N'Bùi Ngọc Anh', N'0915343020', N'', N' Số 100 lô M2 ngõ 35 Trần Kim Xuyến, Yên Hòa, Cầu Giấy, Hà Nội', 0, 0, CAST(N'2025-04-24T14:35:57.7562617' AS DateTime2), CAST(N'2025-04-24T14:35:57.7562622' AS DateTime2), 0)
GO
SET IDENTITY_INSERT [dbo].[ListProducts] ON 


INSERT [dbo].[ListProducts] ([ListProductId], [UserID], [ListProductName]) VALUES (10, N'bc8e28b1-8943-4282-b66e-2fdeff674fea', N'Flash sale 24-5')
INSERT [dbo].[ListProducts] ([ListProductId], [UserID], [ListProductName]) VALUES (14, N'bc8e28b1-8943-4282-b66e-2fdeff674fea', N'Săn sale tối 26-4')
INSERT [dbo].[ListProducts] ([ListProductId], [UserID], [ListProductName]) VALUES (1015, N'bc8e28b1-8943-4282-b66e-2fdeff674fea', N'Săn sale 28-4')
SET IDENTITY_INSERT [dbo].[ListProducts] OFF
GO
SET IDENTITY_INSERT [dbo].[LiveStreamCustomers] ON 


INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1, N'122164436906331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (2, N'122162276552331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (3, N'122162276552331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (4, N'122162276552331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (5, N'122162276552331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (6, N'122164715744331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (7, N'122164716896331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (8, N'122164839326331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (9, N'122164842248331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (10, N'122164842248331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (11, N'122164872086331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (12, N'122164872086331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (13, N'122164872086331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (14, N'122164875182331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (15, N'122164875182331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (16, N'122164875182331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (17, N'122164875182331196', N'9849276838426644')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (18, N'122164876988331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (19, N'122164876988331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (20, N'122164876988331196', N'9849276838426644')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (21, N'122164876988331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (22, N'122164875182331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (23, N'122164876988331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (24, N'122164872086331196', N'9849276838426644')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (25, N'122164926728331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (26, N'122164926728331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (27, N'122164926728331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (28, N'122164926728331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (29, N'122164929794331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (30, N'122164929794331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (31, N'122164937738331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (32, N'122164937738331196', N'9170304843076058')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (33, N'122164938386331196', N'9170304843076058')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (34, N'122164938386331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (35, N'122164938386331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (36, N'122164938386331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (37, N'122164956014331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (38, N'122165043476331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (39, N'122165043476331196', N'10096417470402179')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (40, N'122165044898331196', N'10096417470402179')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (41, N'122165044898331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (42, N'122165044898331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (43, N'122165046350331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (44, N'122165046350331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (45, N'122165046350331196', N'9535928183191974')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (46, N'122165047484331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (47, N'122165048222331196', N'9570129896356691')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (48, N'122165049944331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (49, N'122165077316331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (50, N'122165078546331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (51, N'122165089646331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (52, N'122165089646331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (53, N'122165089646331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (54, N'122165089646331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (55, N'122165093258331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (56, N'122165093258331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (57, N'122165093258331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (58, N'122165093258331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (59, N'122151053540331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (60, N'122151053540331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (61, N'122151053540331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (62, N'122151053540331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (63, N'122151053540331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (64, N'122151053540331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (65, N'122165099630331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (66, N'122165099630331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (67, N'122165099630331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (68, N'122165099630331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (69, N'122165106392331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (70, N'122165106392331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (71, N'122165207900331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (72, N'122165207900331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (73, N'122165211110331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (74, N'122165211110331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (75, N'122165211110331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (76, N'122165211110331196', N'9570129896356691')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (77, N'122165211110331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (78, N'122165211110331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (79, N'122165211974331196', N'266349363239226')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (80, N'122165211974331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (81, N'122165211974331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (82, N'122165211974331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (83, N'122165211974331196', N'9849276838426644')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (84, N'122165211974331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (85, N'122165211974331196', N'9570129896356691')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (86, N'122165211974331196', N'9535928183191974')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (87, N'122165215958331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (88, N'122165216768331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1071, N'122165467826331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1072, N'122165467826331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1073, N'122165467826331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1074, N'122165467826331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1075, N'122165478938331196', N'9239312666134797')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1076, N'122165478938331196', N'7552312751525534')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1077, N'122165478938331196', N'9404298646256687')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1078, N'122165478938331196', N'28407649178881320')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1079, N'122165478938331196', N'28115740348070899')
INSERT [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId], [LivestreamID], [CustomerID]) VALUES (1080, N'122165478938331196', N'9849276838426644')
SET IDENTITY_INSERT [dbo].[LiveStreamCustomers] OFF
GO
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122151053540331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1347523583273065', N'Tom & Jerry 😺🐭| Nibbles’ Diwali Surprise for Tom! 🧨🤪| Cartoon for Kids | #diwalispecial | ‪@cnindia‬', CAST(N'2025-01-16T14:46:49.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122162272154331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1353214802687409', N'share screen', CAST(N'2025-04-08T15:20:09.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122162274482331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1946417916166262', N'tet', CAST(N'2025-04-08T15:40:43.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122162276552331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/2013529655806022', N'test1', CAST(N'2025-04-08T15:49:14.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164436906331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/981287650659327', N'Untitled', CAST(N'2025-04-21T17:37:23.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164715744331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1214821850656120', N'Buổi livestream thử', CAST(N'2025-04-23T15:18:56.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164716896331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1373913337259713', N'LIvestream thu 2', CAST(N'2025-04-23T15:24:24.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164839326331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1127442832480132', N'Livestream1', CAST(N'2025-04-24T09:26:06.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164840082331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1185618226690438', N'Untitled', CAST(N'2025-04-24T09:29:15.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164842248331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/2396079117439198', N'This is test livestream in phone', CAST(N'2025-04-24T09:46:03.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164872086331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1052892650035808', N'Săn sale cuối tháng', CAST(N'2025-04-24T14:05:32.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164875182331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1873357960081608', N'Untitled', CAST(N'2025-04-24T14:24:02.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164876988331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/9960146630714785', N'Untitled', CAST(N'2025-04-24T14:39:32.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164926728331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/660697383556750', N'Untitled', CAST(N'2025-04-24T21:18:08.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164929794331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/643745255200836', N'Untitled', CAST(N'2025-04-24T21:38:25.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164937738331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/1361375685178654', N'Untitled', CAST(N'2025-04-24T22:37:23.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164938386331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1405707520555010', N'Untitled', CAST(N'2025-04-24T22:41:37.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122164956014331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/3932880747040476', N'test', CAST(N'2025-04-25T01:13:41.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165043476331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/663441859726132', N'Săn sale ngày 25/4', CAST(N'2025-04-25T16:27:38.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165044898331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/3560107860961290', N'Phiên live chiều ngày 25/04', CAST(N'2025-04-25T16:37:53.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165046350331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/1362899964856870', N'Phiên live ngày 25/04 siêu ưu đãi', CAST(N'2025-04-25T16:45:43.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165047484331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/686305647225688', N'Cuối tháng săn đồ giá rẻ ngày 25/4', CAST(N'2025-04-25T16:56:57.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165048222331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/700344615888479', N'Live live live mọi người vào live em đi ạ', CAST(N'2025-04-25T17:05:05.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165049944331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1008609687603535', N'Bán dụng cụ học tập ngày 25/04', CAST(N'2025-04-25T17:26:23.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165077316331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/2191324188004453', N'Livestream tối 25-4', CAST(N'2025-04-25T21:03:59.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165078546331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/486374971131519', N'Livestream2 tối 25', CAST(N'2025-04-25T21:14:51.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165089646331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/1592503564702625', N'Live stream đêm 25/4', CAST(N'2025-04-25T23:01:38.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165093258331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/677180408277938', N'Live đêm 25/4/2025', CAST(N'2025-04-25T23:31:49.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165099630331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1737092183686750', N'Fix bug 26/4', CAST(N'2025-04-26T00:36:44.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165106392331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1889219425248749', N'Fix bug', CAST(N'2025-04-26T01:44:23.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165206712331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 10, N'https://www.facebook.com/122153392886331196/videos/702878412264454', N'Săn 26/4', CAST(N'2025-04-26T21:22:56.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165207900331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 14, N'https://www.facebook.com/122153392886331196/videos/1180235143794968', N'Săn sale 26/4', CAST(N'2025-04-26T21:31:32.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165211110331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1720448455516502', N'Tri Thức Hòa Cùng Tự Hào – Sale Đồ Dùng Học Tập Ngập Tràn 26/04!', CAST(N'2025-04-26T22:02:19.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165211974331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/9358466337612802', N'Tri Thức Hòa Cùng Tự Hào – Sale Đồ Dùng Học Tập Ngập Tràn 26/04!', CAST(N'2025-04-26T22:12:31.0000000' AS DateTime2), N'VOD', 0, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165215958331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1439825920702639', N'Fixx bug', CAST(N'2025-04-26T23:01:14.0000000' AS DateTime2), N'VOD', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165216768331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1320231362386280', N'Fixx bug2', CAST(N'2025-04-26T23:08:49.0000000' AS DateTime2), N'LIVE', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165467826331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', NULL, N'https://www.facebook.com/122153392886331196/videos/1721266082076842', N'Live sale ngày 28/4', CAST(N'2025-04-28T14:28:01.0000000' AS DateTime2), N'LIVE', 1, NULL)
INSERT [dbo].[LiveStreams] ([LivestreamID], [UserID], [ListProductID], [StreamURL], [StreamTitle], [StartTime], [Status], [StatusDelete], [PriceMax]) VALUES (N'122165478938331196', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', 1015, N'https://www.facebook.com/122153392886331196/videos/2557515231281729', N'Săn Sale ngày 28/4', CAST(N'2025-04-28T15:46:00.0000000' AS DateTime2), N'VOD', 0, CAST(130000.00 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 


INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (30, CAST(N'2025-04-24T14:41:59.0000000' AS DateTime2), 3, 2, 11, N'9960146630714785_1304892867287064', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (31, CAST(N'2025-04-24T14:45:41.0000000' AS DateTime2), 5, 3, 11, N'9960146630714785_1299641641132372', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (32, CAST(N'2025-04-24T14:45:50.0000000' AS DateTime2), 2, 1, 11, N'9960146630714785_1308057650256590', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (33, CAST(N'2025-04-24T14:46:13.0000000' AS DateTime2), 4, 1, 11, N'9960146630714785_1733535010565890', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (34, CAST(N'2025-04-24T14:46:53.0000000' AS DateTime2), 0, 2, 11, N'9960146630714785_2172359493201824', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (35, CAST(N'2025-04-24T19:36:33.0000000' AS DateTime2), 3, 1, 11, N'2396079117439198_1497891231184883', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (36, CAST(N'2025-04-24T21:22:40.0000000' AS DateTime2), 1, 1, 12, N'660697383556750_1887866081986022', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (37, CAST(N'2025-04-24T21:44:01.0000000' AS DateTime2), 0, 2, 12, N'643745255200836_971282961878321', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (38, CAST(N'2025-04-24T21:24:45.0000000' AS DateTime2), 0, 1, 12, N'660697383556750_555514947593433', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (39, CAST(N'2025-04-24T21:24:50.0000000' AS DateTime2), 0, 1, 12, N'660697383556750_1025714339622367', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (40, CAST(N'2025-04-24T22:47:33.0000000' AS DateTime2), 4, 1, 13, N'1405707520555010_3624741914491081', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (41, CAST(N'2025-04-24T22:50:56.0000000' AS DateTime2), 3, 1, 11, N'1405707520555010_1022060326547223', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (42, CAST(N'2025-04-24T23:14:57.0000000' AS DateTime2), 5, 1, 11, N'1405707520555010_640930882095958', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (43, CAST(N'2025-04-25T16:30:44.0000000' AS DateTime2), 0, 1, 10, N'663441859726132_1353000822657092', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (44, CAST(N'2025-04-25T16:35:01.0000000' AS DateTime2), 0, 1, 10, N'663441859726132_1221862166228031', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (45, CAST(N'2025-04-25T16:35:59.0000000' AS DateTime2), 0, 3, 10, N'663441859726132_1401893447919560', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (47, CAST(N'2025-04-25T16:39:28.0000000' AS DateTime2), 0, 1, 10, N'3560107860961290_1060128346178254', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (48, CAST(N'2025-04-25T16:30:59.0000000' AS DateTime2), 0, 1, 10, N'663441859726132_643097962038046', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (50, CAST(N'2025-04-25T16:44:09.0000000' AS DateTime2), 0, 1, 10, N'3560107860961290_533321829622023', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (51, CAST(N'2025-04-25T16:46:40.0000000' AS DateTime2), 0, 1, 10, N'1362899964856870_1650865072218524', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (52, CAST(N'2025-04-25T16:47:12.0000000' AS DateTime2), 0, 1, 10, N'1362899964856870_986953920177435', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (53, CAST(N'2025-04-25T16:57:59.0000000' AS DateTime2), 0, 3, 10, N'686305647225688_1471421810336465', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (54, CAST(N'2025-04-25T16:56:27.0000000' AS DateTime2), 0, 1, 10, N'1362899964856870_2446379882388799', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (55, CAST(N'2025-04-25T17:09:13.0000000' AS DateTime2), 0, 1, 10, N'700344615888479_1417007176330456', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (56, CAST(N'2025-04-25T17:28:12.0000000' AS DateTime2), 0, 1, 10, N'1008609687603535_658745653556318', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (57, CAST(N'2025-04-25T21:16:01.0000000' AS DateTime2), 1, 1, 10, N'486374971131519_1427968721779512', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (58, CAST(N'2025-04-26T00:43:25.0000000' AS DateTime2), 0, 3, 267, N'1737092183686750_1446769206687535', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (59, CAST(N'2025-04-26T00:44:08.0000000' AS DateTime2), 1, 1, 267, N'1737092183686750_1102715348558119', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (61, CAST(N'2025-04-26T00:48:45.0000000' AS DateTime2), 1, 1, 267, N'1737092183686750_1606114636764482', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (63, CAST(N'2025-04-26T00:43:10.0000000' AS DateTime2), 0, 2, 267, N'1737092183686750_678131625164406', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (64, CAST(N'2025-04-26T00:49:22.0000000' AS DateTime2), 0, 3, 314, N'1737092183686750_1247176753410415', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (66, CAST(N'2025-04-26T22:04:16.0000000' AS DateTime2), 0, 1, 393, N'1720448455516502_1207890507788282', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (67, CAST(N'2025-04-26T22:05:32.0000000' AS DateTime2), 0, 1, 393, N'1720448455516502_1600155720662603', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (68, CAST(N'2025-04-26T22:15:43.0000000' AS DateTime2), 0, 10, 393, N'9358466337612802_650682074526826', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (69, CAST(N'2025-04-26T22:15:23.0000000' AS DateTime2), 1, 2, 393, N'9358466337612802_1037977128214834', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (70, CAST(N'2025-04-26T22:16:12.0000000' AS DateTime2), 1, 3, 393, N'9358466337612802_1429643275065398', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (71, CAST(N'2025-04-26T22:16:42.0000000' AS DateTime2), 0, 4, 393, N'9358466337612802_720131937123021', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (72, CAST(N'2025-04-26T22:20:14.0000000' AS DateTime2), 0, 5, 398, N'9358466337612802_2119052491900400', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (73, CAST(N'2025-04-26T22:20:25.0000000' AS DateTime2), 0, 3, 398, N'9358466337612802_706193002082264', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (74, CAST(N'2025-04-26T22:20:32.0000000' AS DateTime2), 1, 5, 398, N'9358466337612802_996093859356991', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (75, CAST(N'2025-04-26T22:23:36.0000000' AS DateTime2), 1, 1, 399, N'9358466337612802_4873144752911523', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (76, CAST(N'2025-04-26T22:23:44.0000000' AS DateTime2), 1, 1, 399, N'9358466337612802_1416706359671522', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (77, CAST(N'2025-04-26T23:03:08.0000000' AS DateTime2), 0, 1, 399, N'1439825920702639_2067392260424263', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (78, CAST(N'2025-04-26T23:03:51.0000000' AS DateTime2), 0, 1, 399, N'1439825920702639_4113060352265155', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (79, CAST(N'2025-04-26T23:04:27.0000000' AS DateTime2), 0, 4, 399, N'1439825920702639_1231956698321556', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (80, CAST(N'2025-04-26T23:05:03.0000000' AS DateTime2), 0, 2, 399, N'1439825920702639_1386889072450459', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (81, CAST(N'2025-04-26T23:09:43.0000000' AS DateTime2), 0, 1, 399, N'1320231362386280_2716139045238562', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1066, CAST(N'2025-04-28T14:36:06.0000000' AS DateTime2), 0, 1, 1393, N'1721266082076842_653230317475165', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1067, CAST(N'2025-04-28T14:37:22.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_1184718916458194', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1068, CAST(N'2025-04-28T14:37:24.0000000' AS DateTime2), 0, 2, 1393, N'1721266082076842_608612651638042', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1069, CAST(N'2025-04-28T14:37:49.0000000' AS DateTime2), 0, 2, 1393, N'1721266082076842_1475118363896312', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1070, CAST(N'2025-04-28T14:38:40.0000000' AS DateTime2), 0, 3, 1396, N'1721266082076842_2111255109389388', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1071, CAST(N'2025-04-28T14:41:50.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_604009015978464', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1072, CAST(N'2025-04-28T14:43:03.0000000' AS DateTime2), 0, 2, 1393, N'1721266082076842_4207097396216605', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1073, CAST(N'2025-04-28T14:43:08.0000000' AS DateTime2), 0, 2, 1393, N'1721266082076842_533925533106833', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1074, CAST(N'2025-04-28T14:44:50.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_1904181346989434', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1075, CAST(N'2025-04-28T14:45:17.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_843939201287412', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1076, CAST(N'2025-04-28T14:47:42.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_999665482347508', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1077, CAST(N'2025-04-28T14:36:02.0000000' AS DateTime2), 0, 1, 1393, N'1721266082076842_1813949909176979', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1078, CAST(N'2025-04-28T14:36:50.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_1019101313078187', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1079, CAST(N'2025-04-28T14:54:19.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_655861043982542', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1080, CAST(N'2025-04-28T15:00:42.0000000' AS DateTime2), 0, 1, 1393, N'1721266082076842_3575052256124383', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1081, CAST(N'2025-04-28T15:00:50.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_690338970316077', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1082, CAST(N'2025-04-28T15:04:59.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_687080473868806', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1083, CAST(N'2025-04-28T15:13:07.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_1595524597786466', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1084, CAST(N'2025-04-28T15:13:54.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_669538159018377', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1085, CAST(N'2025-04-28T15:15:02.0000000' AS DateTime2), 1, 1, 1393, N'1721266082076842_984033153893062', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1086, CAST(N'2025-04-28T16:04:07.0000000' AS DateTime2), 1, 1, 1397, N'2557515231281729_1314244070038237', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1087, CAST(N'2025-04-28T16:04:30.0000000' AS DateTime2), 0, 1, 1393, N'2557515231281729_1363470451366571', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1088, CAST(N'2025-04-28T16:04:52.0000000' AS DateTime2), 0, 1, 1393, N'2557515231281729_572379825891896', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1089, CAST(N'2025-04-28T16:04:34.0000000' AS DateTime2), 0, 1, 1393, N'2557515231281729_1011363821138744', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1090, CAST(N'2025-04-28T16:06:22.0000000' AS DateTime2), 0, 1, 1395, N'2557515231281729_546637511826865', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1091, CAST(N'2025-04-28T16:06:37.0000000' AS DateTime2), 0, 20, 1396, N'2557515231281729_3136588586498917', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1092, CAST(N'2025-04-28T16:04:01.0000000' AS DateTime2), 1, 3, 1397, N'2557515231281729_2050429965466939', NULL, NULL, 0, NULL)
INSERT [dbo].[Orders] ([OrderID], [OrderDate], [Status], [Quantity], [ProductID], [CommentID], [CurrentPrice], [Note], [StatusCheck], [TrackingNumber]) VALUES (1093, CAST(N'2025-04-28T16:08:14.0000000' AS DateTime2), 1, 13, 1396, N'2557515231281729_1196783565189839', NULL, NULL, 0, NULL)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductListProducts] ON 


INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (127, 267, 10)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (128, 314, 10)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (131, 393, 14)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (132, 394, 14)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (133, 395, 14)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (134, 396, 14)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (135, 397, 14)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (136, 398, 14)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (137, 399, 14)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (138, 400, 14)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (1129, 393, 10)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (1135, 1397, 1015)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (1136, 1393, 1015)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (1137, 1394, 1015)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (1138, 1395, 1015)
INSERT [dbo].[ProductListProducts] ([ProductListProductID], [ProductID], [ListProductID]) VALUES (1139, 1396, 1015)
SET IDENTITY_INSERT [dbo].[ProductListProducts] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 


INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (10, N'Bút lông', N'B222', N'https://res.cloudinary.com/loms/image/upload/v1745647513/rr0pmutpyopilkg55zt4.jpg', N'Bút lông có thể xóa ', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 12, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (11, N'Máy in nhiệt', N'M250', N'https://res.cloudinary.com/loms/image/upload/v1745608765/fr3ojnwptfokm4l4u6oq.jpg', N'Máy in nhiệt kết nối Bluetooth', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(250000.00 AS Decimal(18, 2)), 14, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (12, N'Tai nghe AirPods 4', N'T100', N'https://res.cloudinary.com/loms/image/upload/v1745602091/jt2ytx0xike456sjxoro.png', N'Tai nghe AirPods 4, máy nghe nhạc không dây', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(500000.00 AS Decimal(18, 2)), 10, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (13, N'Sạc dự phòng', N'S201', N'https://res.cloudinary.com/loms/image/upload/v1745588344/q5vcunkpbfiqqcz9o6ko.jpg', N'Sạc dự phòng 10000mAh, dung lượng cao', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(100000.00 AS Decimal(18, 2)), 10, 0)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (14, N'Găng tay cảm ứng', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745557606/op4q6ofqdhvny74ffsrw.webp', N'Sử dụng khi đi xe máy, tính năng cảm ứng', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(100000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (15, N'Găng tay chống nắng', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745557679/i8djelto2ksek8k1ea0y.webp', N'Bảo vệ bàn tay khỏi tia uv, chất liệu da', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(150000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (16, N'Găng tay len', NULL, N'https://m.media-amazon.com/images/I/81niOuYntgL._AC_SL1500_.jpg', N'Chất liệu từ len, giữ ấm cho mùa đông', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(145000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (17, N'Găng tay len (cao cấp)', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745557931/mdr7d8wpxwrdfnej7t7i.webp', N'Chất liệu từ len, giữ ấm cho mùa đông', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(160000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (18, N'Găng tay phượt', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558000/sn6oawk6byofecj2jma6.webp', N'Chất liệu da cap cấp, phù hợp đi phượt', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(260000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (19, N'Găng tay hở ngón', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558086/wusmq3jkek95xt5l2mwg.webp', N'Chất liệu da lộn cao cấp', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(260000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (20, N'Găng tay chống nước', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558141/c2lmo9cudmybidi4uwud.webp', N'Sử dụng khi đi xe máy, có thể chống nước', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(160000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (21, N'Ba lô dây rút', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558227/xrpvyos2jfv8vvq6kaju.webp', N'Nhỏ gọn , tiện lợi khi mang đồ', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(100000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (22, N'Túi bao tử đeo bụng', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558306/i5oag0gpuicyt8s7pw4p.webp', N'Nhỏ gọn, tiện lợi khi mang theo', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(150000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (23, N'Túi da', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558365/lz283p11tzadtpf3vyyk.webp', N'Chấtt liệu da, phù hợp khi đi chơi', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(250000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (24, N'Túi đeo bụng chạy bộ', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558449/sioyye5b0q9cuyt9yjil.webp', N'Chấtt liệu dù, phù hợp khi chạy bộ', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(150000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (25, N'Túi đeo chéo', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558492/dwwz8fhov9hqplvggghy.webp', N'Chấtt liệu vải canvas, tiện lợi khi mang theo', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(150000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (26, N'Túi đeo chéo nhật bản', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558552/ieps2icsfkra3q0jj4sh.webp', N'Xuất sứ nhật bản', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(250000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (27, N'Túi đeo chéo da', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558614/wfclld19xplvb76akelc.webp', N'Thiết kế nhỏ gọn, đẹp mắt', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(250000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (28, N'Túi hộp', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558657/wzyrxdk4zm48yjxfwz5f.webp', N'Thiết kế tỉ mỉ, sang trọng', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(250000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (29, N'Túi đeo hông', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558706/g4vvishysvoom49mchfh.webp', N'Sử dụng cho cả nam và nữ', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(180000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (30, N'Túi nam', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558804/mukkuyxhiyqszaltq3k4.webp', N'Có thể để đồ kích thước vừa, phù hợp mọi lứa tuổi', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(180000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (31, N'Áo chống nắng', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558889/sjyvje5bjok1gapjhjn5.webp', N'Làm từ chất liệu cao cấp, tránh tia uv tiếp xúc cơ thể', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(200000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (32, N'Ba lô thể thao', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745558949/uxtelcq50juyxjyxs21d.webp', N'Kích thước lơn, để được nhiều đồ', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(200000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (33, N'Bảng chống đẩy thể thao', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559037/rty7tbkyhsnjg2puqelh.webp', N'Tập luyện, rèn luyện sức khỏe', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(200000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (34, N'Băng đô thể thao', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559092/xr7oyvtsmpqergfbrvf4.webp', N'Thấm hút mồ hôi, phù hợp khi tập luyện', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(100000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (35, N'Băng đô thể thao', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559155/dtinsqxjb99cwt470pdv.webp', N'Chất liệu cao cấp, dùng khi chơi thể thao', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(150000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (36, N'Băng keo thể thao', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559233/ygo8qdt7ku30gnne8va4.webp', N'Sử dụng để giảm chấn thương khi tập luyện', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(50000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (37, N'Bó gối thể thao', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559276/oisyk7uzyo7n0qa7lbni.webp', N'Sử dụng để giảm chấn thương đầu gối khi tập luyện', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(150000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (38, N'Đai cổ chân thể thao', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559326/munlltxmaivrngsnuge2.webp', N'Giảm chấn thương cổ chân khi luyện tập', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(100000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (39, N'Giày beta', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559368/yxnp6pzwlgl3tmbej1p9.webp', N'Sử dụng khi luyện tập', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(100000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (40, N'Giày đá bóng CT3', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559405/chxitqpm895bmevpwjkx.webp', N'Dùng cho đá bóng', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(180000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (41, N'Giày đá bóng bata', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559454/au9g2tcvyjzfp9tmnibx.webp', N'Sử dụng chất liệu cao cấp, dùng cho đá bóng', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(180000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (42, N'Giày đá bóng NEMEZIZ', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559517/bmwgazasa1s0i56janzk.webp', N'Sử dụng chất liệu cao cấp, dùng cho đá bóng', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(400000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (43, N'Giày đá bóng Vapor', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559566/fopxo7uizfk7beoutjwc.webp', N'Sử dụng chất liệu cao cấp, dùng cho đá bóng', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(400000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (44, N'Lót giày thể thao', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559610/odgwy9eon65qugrr7czv.webp', N'Chất liệu cao cấp, êm ái khi sử dụng', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(100000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (45, N'Set 2 vợt cầu lông', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559679/yyhwsfzlauhgh8wdpqde.webp', N'Chất liệu cao cấp', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(250000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (46, N'Tất đá bóng', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559726/qle34gt2mzr2jd9wrodn.webp', N'Chống trơn khi chơi thể thao', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(100000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (47, N'Tất thể thao', NULL, N'https://res.cloudinary.com/loms/image/upload/v1745559765/oqht2lxwo394eakfeisg.webp', N'Dùng cho chơi thể thao', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(120000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (267, N'But bi Thien Long', N'BTL001', N'https://product.hstatic.net/1000230347/product/tl-063_eco_2_f27423a3db1e4d90a57effab1237ca03_large.jpg', N'But bi mau xanh, muc em, viet muot', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(5000.00 AS Decimal(18, 2)), 87, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (283, N'Vo ke ngang 96 trang', N'VKH002', N'https://vanphongphambanhat.com.vn/wp-content/uploads/2022/07/tap-ke-ngang-vpp-ba-nhat-3.jpg', N'Vo hoc sinh, giay trang, ke ngang ro', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(12000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (288, N'Cap sach hoc sinh', N'CSH003', N'https://bizweb.dktcdn.net/100/343/358/files/balo-hoc-sinh-912-st-xanh-den-khung-long-2.jpg?v=1673686615909', N'Cap sach mau do, co ngan dung binh nuoc', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(150000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (289, N'But chi go 2B', N'BCG004', N'https://product.hstatic.net/1000362139/product/but_chi_classmate_pc102_7a39ea01c0f34ed28894501458f188b5.jpg', N'But chi 2B, de got, viet mem', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(3000.00 AS Decimal(18, 2)), 200, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (292, N'Tay but chi Pentel', N'TBC005', N'https://down-vn.img.susercontent.com/file/3a84e5d3eb4e4ef1a8c9ef4a88e88250', N'Tay trang, khong de lai vet ban', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(8000.00 AS Decimal(18, 2)), 150, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (294, N'Hop but vai', N'HBV006', N'https://down-vn.img.susercontent.com/file/sg-11134201-22120-l3rmr7lfw0kv36', N'Hop but vai nhieu ngan, mau xanh duong', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(25000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (295, N'Thuoc ke 20cm', N'TK007', N'https://product.hstatic.net/1000362139/product/thuoc_20cm_tq_6ed1c8e1f8ad4796b4925c361bdccb05.jpg', N'Thuoc nhua trong suot, chia vach ro', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(5000.00 AS Decimal(18, 2)), 80, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (296, N'But long bang trang', N'BLB008', N'https://anlocviet.vn/upload/product/butlongbangthienlongwb031-6322.jpg', N'But long mau den, de xoa', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (297, N'Bang viet tay nho', N'BVT009', N'https://bizweb.dktcdn.net/100/146/976/products/bang-tu-trang-0-6x0-8m-6c5f7a1e-9b4c-4492-b9e8-f2f78c2b6c87.jpg?v=1685454923833', N'Bang viet tay kich thuoc 20x30cm', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(35000.00 AS Decimal(18, 2)), 25, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (298, N'Giay note 3x3cm', N'GN010', N'https://anlocviet.com/wp-content/uploads/Giay-note-vang-3x3-Pronoti-an-loc-viet.jpg', N'Giay note mau vang, co keo dinh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (299, N'But highlight Stabilo', N'BHL011', N'https://product.hstatic.net/200000350979/product/-recovered-recovered-recovered-recovered-recovered-recovered-recovered_f497bed7f66b4a51b0b45d9aaedf84fc_master.png', N'But highlight mau vang, muc sang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(20000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (300, N'So tay A5', N'STA012', N'https://product.hstatic.net/200000033050/product/so-tay-notebook-a5-da-pu_e9706d560de94f4aa95d32a7f4b71d65_1024x1024.jpg', N'So tay bia cung, 100 trang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(30000.00 AS Decimal(18, 2)), 35, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (301, N'Hop mau nuoc 12 mau', N'HMC013', N'https://product.hstatic.net/1000230347/product/artboard_1_015d18c175b04fa58b16b4b84c8f417f.jpg', N'Hop mau nuoc, di kem co', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(45000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (302, N'But gel 0.5mm', N'BGL014', N'https://product.hstatic.net/1000362139/product/deli_a575_788400140443458285658e971a6135d0.jpg', N'But gel mau den, net manh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 90, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (303, N'Keo dan giay', N'KDG015', N'https://queenstationery.com.vn/vnt_upload/product/12_2021/1_2.png', N'Keo dan giay dang thoi, de su dung', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(7000.00 AS Decimal(18, 2)), 120, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (304, N'May tinh Casio fx-570', N'MTC016', N'https://product.hstatic.net/1000330808/product/fx-570es_plus-2_cy-844a_f_copy_f7d417f2dec0473fa0cb8e4a436753a0.png', N'May tinh khoa hoc, nhieu chuc nang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(250000.00 AS Decimal(18, 2)), 15, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (305, N'But xoa keo', N'BXK017', N'https://product.hstatic.net/1000230347/product/artboard_5_copy_3-1_80faaddbf7f24f36857b693ec926d32e_1024x1024.jpg', N'But xoa keo, de su dung, kho nhanh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(12000.00 AS Decimal(18, 2)), 70, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (306, N'Balo hoc sinh', N'BHS018', N'https://product.hstatic.net/1000379600/product/anhbia_b3010aa831b0418fb5855898a0cbd26a.jpg', N'Balo hoc sinh, nhieu ngan, mau xanh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(200000.00 AS Decimal(18, 2)), 10, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (307, N'Hop but nhua', N'HBN019', N'https://bizweb.dktcdn.net/thumb/1024x1024/100/334/874/products/3362.jpg?v=1594957725403', N'Hop but nhua cung, mau trong suot', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (308, N'Thuoc do goc', N'TDC020', N'https://sonca.vn/wp-content/uploads/2023/08/thuoc-do-180-do-1200x900.jpg', N'Thuoc do goc bang nhua, chia do chinh xac', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(8000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (309, N'But chi mau 12 mau', N'BCC021', N'https://product.hstatic.net/1000230347/product/artboard_3_61c53bc444864ed3a9a11b425ee7200a_1024x1024.jpg', N'But chi mau, 12 mau sac tuoi sang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(30000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (310, N'So tay ke o', N'STK022', N'https://down-vn.img.susercontent.com/file/7c7ab4bef0f21ad624e3578f0952c703', N'So tay ke o, bia mem, 80 trang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(20000.00 AS Decimal(18, 2)), 45, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (311, N'Bang keo trong', N'BKT023', N'https://product.hstatic.net/1000230347/product/keo_trong_150y_1cuon_90fd3c327b6c461ab19128be26b24ab6.jpg', N'Bang keo trong suot, chieu dai 50m', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 80, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (312, N'Keo hoc sinh', N'KHS024', N'https://product.hstatic.net/1000362139/product/deli_6023_198f40f5e8674ebabd3331b87fd53307.jpg', N'Keo hoc sinh, luoi an toan', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(12000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (313, N'Hop dung do dung', N'HDD025', N'https://product.hstatic.net/1000253397/product/hop_do_dung_hoc_tap__1__result_32bd1b9823524c79a4d32cd5320e8439_1024x1024.jpg', N'Hop dung do dung hoc tap, nhieu ngan', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(40000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (314, N'But bi 10 mau', N'BBM026', N'https://bizweb.dktcdn.net/thumb/grande/100/379/648/files/20240512-121836.jpg?v=1715491158771', N'But bi 10 mau, tien loi cho hoc tap', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(25000.00 AS Decimal(18, 2)), 27, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (315, N'Vo ke ngang 200 trang', N'VKH027', N'https://klong.com.vn/image/cache/catalog/Vo%20ke%20ngang/ms%20294-800x800.jpg', N'Vo hoc sinh, 200 trang, bia cung', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(25000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (316, N'But da quang 2 dau', N'BDQ028', N'https://bizweb.dktcdn.net/100/464/700/products/but-da-quang-hl03-1.jpg?v=1703055735127', N'But da quang 2 dau, mau hong va vang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (317, N'Cap sach chong gu', N'CSG029', N'https://bizweb.dktcdn.net/thumb/large/100/343/358/products/z5644971380257-24088d904a54e7875d3f85ba05434ef9.jpg?v=1721287167943', N'Cap sach chong gu, ho tro cot song', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(300000.00 AS Decimal(18, 2)), 10, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (318, N'But chi kim 0.5mm', N'BCK030', N'https://product.hstatic.net/1000362139/product/deli_64922_a3f3c292b7724b8785fd3542ed95a279.jpg', N'But chi kim 0.5mm, mau den', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 70, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (319, N'Ngoi chi 0.5mm', N'NCH031', N'https://product.hstatic.net/1000039248/product/capture_2a7be93a38f54b79bb1f37f5195cf525_1024x1024.jpg', N'Ngoi chi 0.5mm, hop 60 ngoi', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (320, N'Bang trang lon', N'BTL032', N'https://banghoanggia.vn/wp-content/uploads/2021/12/bang-banh-xe-day-scaled.jpg', N'Bang trang lon, kich thuoc 60x90cm', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(150000.00 AS Decimal(18, 2)), 5, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (321, N'Khan lau bang', N'KLB033', N'https://thegioibang.com/wp-content/uploads/2021/06/Khan-Microfiber-e1655345340338.png', N'Khan lau bang, chat lieu mem', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(5000.00 AS Decimal(18, 2)), 80, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (322, N'Hop but kim loai', N'HBK034', N'https://malonedc.com/wp-content/uploads/2024/11/Hop-Dung-But-Bang-Kim-Loai-1.jpg', N'Hop but kim loai, mau bac', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(30000.00 AS Decimal(18, 2)), 25, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (323, N'Thuoc ke 30cm', N'TK035', N'https://chuyenvanphongpham.com/upload/product/thuoc-ke-deo-winq-30cm_7125.jpg', N'Thuoc ke 30cm, nhua trong suot', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(7000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (324, N'But bi 0.7mm', N'BB036', N'https://product.hstatic.net/1000362139/product/tl0368_cc3fd71c54b043c6ac39c5f5e17711c7.jpg', N'But bi 0.7mm, mau do', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(5000.00 AS Decimal(18, 2)), 90, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (325, N'So tay bia da', N'STB037', N'https://quatangthanhphat.com/wp-content/uploads/2021/10/so-tay-bia-da-strong-da-lon.jpg', N'So tay bia da, 120 trang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(50000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (326, N'Hop mau sap 24 mau', N'HCS038', N'https://down-vn.img.susercontent.com/file/bd17c2c2c5f3dfda76f35afd079b8de8', N'Hop mau sap, 24 mau sac ruc ro', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(35000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (327, N'But xoa nuoc', N'BXN039', N'https://product.hstatic.net/1000230347/product/artboard_4_51102c93f6c842408b71e570c242c98c.jpg', N'But xoa nuoc, muc trang, kho nhanh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (328, N'Cap sach in hinh', N'CSH040', N'https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-lttost9o3a4t1b', N'Cap sach in hinh hoat hinh, mau xanh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(180000.00 AS Decimal(18, 2)), 15, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (329, N'But long mau 12 mau', N'BLM041', N'https://anlocviet.vn/upload/product/but-long-mau-12-mau-vi-4553.jpg', N'But long mau, 12 mau, dau manh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(40000.00 AS Decimal(18, 2)), 25, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (330, N'Vo ke ngang 120 trang', N'VKH042', N'https://bizweb.dktcdn.net/100/334/874/products/vo-ke-ngang-120-trang-hong-ha-2-21c0f844-bd43-4aa6-a247-dba5c6465ed5.jpg?v=1722671317903', N'Vo ke ngang, 120 trang, bia mem', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (331, N'But da quang 5 mau', N'BDQ043', N'https://product.hstatic.net/1000230347/product/50005243_2ddd3be80b8a400586876132bff26fd1.jpg', N'But da quang, 5 mau sac khac nhau', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(25000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (332, N'Thuoc ke hinh hoc', N'TKH044', N'https://down-vn.img.susercontent.com/file/f1d3198fe5d693124a9cc699f77fb910', N'Thuoc ke hinh hoc, nhieu hinh dang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (333, N'Balo chong tham', N'BCT045', N'https://www.nature-hike.vn/wp-content/uploads/2023/06/z4389322282554_d51e0d701536778b79a13fa961af0222.jpg', N'Balo chong tham nuoc, mau den', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(250000.00 AS Decimal(18, 2)), 10, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (334, N'Hop but 2 ngan', N'HBN046', N'https://product.hstatic.net/200000599493/product/o1cn01sleevm1i1yafopxkt___4161970833.jpg_430x430q90_349d6c9a832e405e80798393d845e650_large.jpg', N'Hop but 2 ngan, mau hong', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(20000.00 AS Decimal(18, 2)), 35, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (335, N'But chi kim 0.7mm', N'BCK047', N'https://product.hstatic.net/1000362139/product/but_chi_kim_777_ad1faae1618a45b3b03dabaf3ba8f37d.jpg', N'But chi kim 0.7mm, mau xanh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (336, N'Ngoi chi 0.7mm', N'NCH048', N'https://product.hstatic.net/1000362139/product/but_chi_kim_777_ad1faae1618a45b3b03dabaf3ba8f37d.jpg', N'Ngoi chi 0.7mm, hop 60 ngoi', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(12000.00 AS Decimal(18, 2)), 80, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (337, N'Bang trang nho', N'BTN049', N'https://thegioibang.com/wp-content/uploads/2021/07/Bang-tu-trang-Easyboard-Vui-hoc-cung-Cu-Ollive-3-800x800.png', N'Bang trang nho, kich thuoc 30x40cm', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(50000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (338, N'But bi 4 mau', N'BBM050', N'https://product.hstatic.net/1000362139/product/but_bi_4_mau_cce615e99b414e938ba5cd9f81f5cc87_grande.jpg', N'But bi 4 mau, tien loi cho hoc tap', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (339, N'But muc Pilot', N'BMP051', N'https://product.hstatic.net/1000083887/product/6afb6cc5ff72f2b2bb30a755c9f6fff6_08307cf577e04e4480ab69b18df9fdd8.jpg', N'But muc Pilot, mau xanh, viet muot', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(30000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (340, N'So tay A6', N'STA052', N'https://product.hstatic.net/1000403382/product/so_tay_a6__1__3b8387e19d3c473b926fa05bdd3b5bd9_master.jpg', N'So tay A6, bia mem, 80 trang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (341, N'Hop mau nuoc 24 mau', N'HMC053', N'https://artstore.com.vn/wp-content/uploads/2018/02/mau-nuoc-leningrad-5.jpg', N'Hop mau nuoc 24 mau, di kem co', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(60000.00 AS Decimal(18, 2)), 15, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (342, N'But gel 0.7mm', N'BGL054', N'https://down-vn.img.susercontent.com/file/8a6860bd3dbcfc09d5388711a3b7b740', N'But gel mau xanh, net manh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 90, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (343, N'Keo dan da nang', N'KDD055', N'https://product.hstatic.net/1000230347/product/fo-sg001_vn_83deacf980d746a4b93406ea55b88577.jpg', N'Keo dan da nang, dinh chac', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (344, N'May tinh Casio fx-991', N'MTC056', N'https://images-cdn.ubuy.pe/6522e92778fe2f610663f908-casio-fx-991cw-classwiz-non-programmable.jpg', N'May tinh khoa hoc, ho tro nhieu phep tinh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(350000.00 AS Decimal(18, 2)), 10, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (345, N'But xoa nuoc 2 dau', N'BXN057', N'https://product.hstatic.net/1000230347/product/artboard_7_e0563aef894c45408080c5dc11e26600.jpg', N'But xoa nuoc 2 dau, muc trang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (346, N'Balo hoc sinh lon', N'BHL058', N'https://www.boxmart.vn/upload/202407/balo-hoc-sinh-cap-2-cap-3-suc-chua-lon-cl6699.jpg', N'Balo hoc sinh lon, mau xanh la', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(220000.00 AS Decimal(18, 2)), 8, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (347, N'Hop but nhua 2 tang', N'HBN059', N'https://down-vn.img.susercontent.com/file/sg-11134201-23010-kjf53vqkc6lv07', N'Hop but nhua 2 tang, mau xanh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(20000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (348, N'Thuoc do goc 180 do', N'TDC060', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSCtKBrXS3Zj8-Zv25ArRYuMdPt0cDYEvY0bQ&s', N'Thuoc do goc 180 do, nhua trong', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (349, N'But chi mau 24 mau', N'BCC061', N'https://product.hstatic.net/1000230347/product/artboard_4_b875df8149c04c72a3e8f31228b05cca.jpg', N'But chi mau, 24 mau sac ruc ro', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(45000.00 AS Decimal(18, 2)), 30, 1)
GO
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (350, N'So tay ke o A4', N'STK062', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR4qrscyBG8szG16JBrhNjOs10lFZ-jNsGHdw&s', N'So tay ke o A4, bia cung, 100 trang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(35000.00 AS Decimal(18, 2)), 25, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (351, N'Bang keo duc', N'BKD063', N'https://bangkeocaonghe.vn/wp-content/uploads/2019/07/bang-dinh-duc-100yard.webp', N'Bang keo duc, chieu dai 50m', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(12000.00 AS Decimal(18, 2)), 70, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (352, N'Keo hoc sinh lon', N'KHL064', N'https://cdn1.fahasa.com/media/catalog/product/6/9/6973157010052-mau1-_4_.jpg', N'Keo hoc sinh lon, luoi an toan', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (353, N'Hop dung do dung lon', N'HDD065', N'https://cuahangdungcu.vn/resources/2022/06/23/thung-dung-do-nghe-co-lon-ktc-ekp-1-a-chdc-4.jpg', N'Hop dung do dung lon, nhieu ngan', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(50000.00 AS Decimal(18, 2)), 15, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (354, N'But bi 6 mau', N'BBM066', N'https://product.hstatic.net/1000362139/product/but_6_mau7_41b72bc18a9845179cc5a9624053319d.jpg', N'But bi 6 mau, tien loi', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(20000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (355, N'Vo ke ngang 250 trang', N'VKH067', N'https://down-vn.img.susercontent.com/file/sg-11134201-7rdwr-lyqnm6mmlcmk22', N'Vo ke ngang, 250 trang, bia cung', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(30000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (356, N'But da quang 3 mau', N'BDQ068', N'https://product.hstatic.net/1000230347/product/artboard_5_9c201f8fe7994f64937faeba723856ca.jpg', N'But da quang 3 mau, mau sac tuoi sang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(20000.00 AS Decimal(18, 2)), 45, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (357, N'Cap sach chong nuoc', N'CSH069', N'https://down-vn.img.susercontent.com/file/74130542392371fd5cb15475e3997572', N'Cap sach chong nuoc, mau hong', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(250000.00 AS Decimal(18, 2)), 12, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (358, N'But chi kim 0.9mm', N'BCK070', N'https://down-vn.img.susercontent.com/file/f1a397edd887d119279c50be53117802', N'But chi kim 0.9mm, mau xam', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (359, N'Ngoi chi 0.9mm', N'NCH071', N'https://down-vn.img.susercontent.com/file/sg-11134201-23010-wdf66hs0damv57', N'Ngoi chi 0.9mm, hop 60 ngoi', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(12000.00 AS Decimal(18, 2)), 80, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (360, N'Bang trang trung', N'BTM072', N'https://down-vn.img.susercontent.com/file/vn-11134207-7ras8-m3vgfx5w5gvped', N'Bang trang trung, kich thuoc 40x60cm', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(80000.00 AS Decimal(18, 2)), 15, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (361, N'Khan lau bang lon', N'KLB073', N'https://thegioibang.com/wp-content/uploads/2021/06/Khan-Microfiber-e1655345340338.png', N'Khan lau bang lon, chat lieu mem', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(8000.00 AS Decimal(18, 2)), 70, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (362, N'Hop but kim loai lon', N'HBK074', N'https://pos.nvncdn.com/ccc5dd-37291/ps/content/20240309_tEFdP2X1.png', N'Hop but kim loai lon, mau vang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(35000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (363, N'Thuoc ke 50cm', N'TK075', N'https://down-vn.img.susercontent.com/file/dccda1f4a409264a1af4e4a531713b42', N'Thuoc ke 50cm, nhua trong suot', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (364, N'But bi 0.5mm', N'BB076', N'https://bizweb.dktcdn.net/100/445/986/products/bgozow0ourelrqwhph-g5q.jpg?v=1692773918313', N'But bi 0.5mm, mau den', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(5000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (365, N'So tay bia da A4', N'STB077', N'https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-lpn93clydlcra4', N'So tay bia da A4, 150 trang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(70000.00 AS Decimal(18, 2)), 15, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (366, N'Hop mau sap 36 mau', N'HCS078', N'https://product.hstatic.net/1000330808/product/_mg_9294_c463eb229ea842a789732930f1b80d12.png', N'Hop mau sap, 36 mau sac ruc ro', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(50000.00 AS Decimal(18, 2)), 25, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (367, N'But xoa nuoc lon', N'BXN079', N'https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-lt2fgq699nkp00_tn.webp', N'But xoa nuoc lon, muc trang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(12000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (368, N'Cap sach thoi trang', N'CSH080', N'https://down-vn.img.susercontent.com/file/a8e61d38e887a4d73359cd77622ff68b', N'Cap sach thoi trang, mau den', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(200000.00 AS Decimal(18, 2)), 10, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (369, N'But long mau 24 mau', N'BLM081', N'https://product.hstatic.net/1000230347/product/artboard_2_copy_66efacfe893c4b9f8f02171e405ae76c.jpg', N'But long mau, 24 mau, dau manh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(60000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (370, N'Vo ke ngang 150 trang', N'VKH082', N'https://product.hstatic.net/1000362139/product/la50017_e54278bb28c149f0a48e7950d0eb1b13.jpg', N'Vo ke ngang, 150 trang, bia mem', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(18000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (371, N'But da quang 6 mau', N'BDQ083', N'https://bizweb.dktcdn.net/100/464/700/products/thiet-ke-chua-co-ten-3-compressed-e8e28571-e6fa-4b7a-a500-ea35efcaab4a.jpg?v=1672709168137', N'But da quang, 6 mau sac khac nhau', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(30000.00 AS Decimal(18, 2)), 35, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (372, N'Thuoc ke nhua deo', N'TKH084', N'https://nhasachphuongnam.com/images/detailed/207/1_3d02-dj.jpg', N'Thuoc ke nhua deo, dai 30cm', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(8000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (373, N'Balo thoi trang', N'BCT085', N'https://www.lottemart.vn/media/catalog/product/cache/0x0/8/9/8936072420380.jpg.webp', N'Balo thoi trang, mau xam', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(280000.00 AS Decimal(18, 2)), 8, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (374, N'Hop but 3 ngan', N'HBN086', N'https://cdn1.fahasa.com/media/catalog/product/8/9/8938500905339-mau2-_2_.jpg', N'Hop but 3 ngan, mau xanh duong', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(25000.00 AS Decimal(18, 2)), 30, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (375, N'But chi kim 0.3mm', N'BCK087', N'https://cdn1.fahasa.com/media/catalog/product/4/9/4902506063595.jpg', N'But chi kim 0.3mm, mau bac', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(20000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (376, N'Ngoi chi 0.3mm', N'NCH088', N'https://down-vn.img.susercontent.com/file/sg-11134201-23010-wdf66hs0damv57', N'Ngoi chi 0.3mm, hop 60 ngoi', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 70, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (377, N'Bang trang lon 2 mat', N'BTL089', N'https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-ltf1jxi5reve7a', N'Bang trang lon 2 mat, 60x90cm', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(200000.00 AS Decimal(18, 2)), 5, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (378, N'But bi 8 mau', N'BBM090', N'https://down-vn.img.susercontent.com/file/991689d727bb8ff3e9c7fc405c1af0b5', N'But bi 8 mau, tien loi', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(25000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (379, N'Vo ke ngang 300 trang', N'VKH091', N'https://bizweb.dktcdn.net/thumb/grande/100/220/344/products/55-cad9ae6e-277b-4302-bff6-088398b58850.jpg?v=1702546345520', N'Vo ke ngang, 300 trang, bia cung', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(35000.00 AS Decimal(18, 2)), 25, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (380, N'But da quang lon', N'BDQ092', N'https://bizweb.dktcdn.net/thumb/grande/100/467/726/products/upload9fbd3c6ae7f243ac854a171a.jpg?v=1686216398197', N'But da quang lon, mau vang', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (381, N'Cap sach sieu nhe', N'CSH093', N'https://assets.aemi.vn/images_resized/2024/8/11/1723394608549-736463', N'Cap sach sieu nhe, mau xanh duong', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(180000.00 AS Decimal(18, 2)), 15, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (382, N'But chi kim 0.7mm lon', N'BCK094', N'https://bizweb.dktcdn.net/100/210/055/products/artdoorbutchibampentela120veky-2eb02e3a-65bd-4b1d-856e-1e84d4c5c162.jpg?v=1722322939797', N'But chi kim 0.7mm lon, mau do', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(20000.00 AS Decimal(18, 2)), 40, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (383, N'Ngoi chi 0.7mm lon', N'NCH095', N'https://down-vn.img.susercontent.com/file/ac3b3b63a8c25caa7be3d0d98f382c5c', N'Ngoi chi 0.7mm lon, hop 100 ngoi', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 60, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (384, N'Bang trang nho 2 mat', N'BTN096', N'https://down-vn.img.susercontent.com/file/sg-11134201-22120-eehy7lcs65kv91', N'Bang trang nho 2 mat, 30x40cm', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(60000.00 AS Decimal(18, 2)), 20, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (385, N'Khan lau bang mini', N'KLB097', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSs_NNM1bqaGIOa0mcksL6Ryr3i440sGbn7EQ&s', N'Khan lau bang mini, chat lieu mem', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(5000.00 AS Decimal(18, 2)), 80, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (386, N'Hop but nhua trong suot', N'HBN098', N'https://bizweb.dktcdn.net/100/421/857/products/z2865484409552-7eccf3400bc698a28a0d0ce5c6570365.jpg?v=1634871894657', N'Hop but nhua trong suot, 1 ngan', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(15000.00 AS Decimal(18, 2)), 50, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (387, N'Thuoc ke 15cm', N'TK099', N'https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lk7ilt96k8hub8', N'Thuoc ke 15cm, nhua trong suot', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(4000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (388, N'But bi 2 mau', N'BBM100', N'https://down-vn.img.susercontent.com/file/vn-11134211-7ras8-m13nma8ik9zzd9', N'But bi 2 mau, do va den', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(8000.00 AS Decimal(18, 2)), 70, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (390, N'123', N'123', N'https://res.cloudinary.com/loms/image/upload/v1745576655/acwh1oervsyfwsrb73dw.webp', N'123', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(27000.00 AS Decimal(18, 2)), 12, 0)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (393, N'Bút bi thiên long xanh', N'BTL150', N'https://res.cloudinary.com/loms/image/upload/v1745678762/mjq2k5icl2xlfsnsvcrf.jpg', N'Nét viết đẹp, sử dụng lâu', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(1500.00 AS Decimal(18, 2)), 72, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (394, N'Bút bi thiên long den', N'BTD150', N'https://res.cloudinary.com/loms/image/upload/v1745678805/wl2xnhsrfllqtgc7zlst.jpg', N'Nét viết đẹp, sử dụng lâu, mực đen', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(1500.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (395, N'Bút geo thien long xanh', N'GTL300', N'https://res.cloudinary.com/loms/image/upload/v1745678892/she8wy6jzxzsocetyxpp.jpg', N'Nét viết đẹp, sử dụng lâu, mực xanh', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(3000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (396, N'Bút chì kim', N'BCK100', N'https://res.cloudinary.com/loms/image/upload/v1745678974/um2c9u4ehyyvcnyshlsi.jpg', N'Nét đẹp, có thể thay đầu chì', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (397, N'Ngòi chì kim', N'NCK200', N'https://res.cloudinary.com/loms/image/upload/v1745679043/crjz7tu5b4qxusfzgkf9.jpg', N'Để thay thế ngòi bút chì kim', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(2000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (398, N'Tẩy', N'T200', N'https://res.cloudinary.com/loms/image/upload/v1745679084/ctu3fijqtneva9xrmt88.jpg', N'Tẩy xóa bút chì', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(2000.00 AS Decimal(18, 2)), 75, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (399, N'Sách A6 klong', N'SA200', N'https://res.cloudinary.com/loms/image/upload/v1745679186/vcac3th5nwwthad3tkyq.jpg', N'Sách kích thước lớn, dùng lâu', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(20000.00 AS Decimal(18, 2)), 39, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (400, N'Giấy vẽ A4', N'GV4', N'https://res.cloudinary.com/loms/image/upload/v1745679271/ekn4l0xfyw3thlzlom9c.jpg', N'Sách kích thước lớn, dùng lâu', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(5000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (1393, N'Ốp điện thoại Iphone13', N'ODT300', N'https://res.cloudinary.com/loms/image/upload/v1745824772/iw9acolbfhwgodmqbpfs.png', N'Bảo vệ điện thoại, giảm va đập', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(30000.00 AS Decimal(18, 2)), 73, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (1394, N'Bấm móng tay', N'BMT100', N'https://res.cloudinary.com/loms/image/upload/v1745824832/w8ipapgrrhagilxpp2os.png', N'Cắt móng', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(10000.00 AS Decimal(18, 2)), 100, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (1395, N'USB', N'USB120', N'https://res.cloudinary.com/loms/image/upload/v1745824883/dcku9todrh56i6iyipva.png', N'Lưu trữ dữ liệu', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(120000.00 AS Decimal(18, 2)), 9, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (1396, N'Bật lửa', N'BL700', N'https://res.cloudinary.com/loms/image/upload/v1745824959/wxo9i5ctr88pmse7p5zi.png', N'Tạo lửa', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(7000.00 AS Decimal(18, 2)), 64, 1)
INSERT [dbo].[Products] ([ProductID], [Name], [ProductCode], [ImageURL], [Description], [UserID], [Price], [Stock], [Status]) VALUES (1397, N'Đồng hồ', N'DH400', N'https://res.cloudinary.com/loms/image/upload/v1745825062/gjuxuvsdhcgt6qkikgvv.png', N'Phụ kiện đeo tay, xem giờ', N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(400000.00 AS Decimal(18, 2)), 78, 1)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
INSERT [dbo].[Users] ([Id], [CreatedAt], [ImageURL], [FullName], [Sex], [Address], [TokenFacbook], [PageId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'6399cb79-2f9e-4b19-ae00-26ff87b9cff4', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, N'Male', NULL, NULL, NULL, N'uuuu', N'UUUU', N'manhpvhe160303@fpt.edu.vn', N'MANHPVHE160303@FPT.EDU.VN', 1, N'AQAAAAIAAYagAAAAEHT8P0ZaQtpDGKVNhHWQcSt0zJ/WED6o2NmjNtsQW/+XbvQAVKz+3AYtObyJx+XMDg==', N'U2TKS4ZFUJ7KRHMJ4M2477I6W73GFARM', N'48717d0e-6da4-46db-a287-1882ee38a470', N'0222222222', 0, 0, NULL, 1, 0)
INSERT [dbo].[Users] ([Id], [CreatedAt], [ImageURL], [FullName], [Sex], [Address], [TokenFacbook], [PageId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'bc8e28b1-8943-4282-b66e-2fdeff674fea', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'https://res.cloudinary.com/loms/image/upload/v1745308344/hbkrmy3zkm1ug1gacl1s.jpg', N'Pham Manh', N'Female', N'Hanoiii', N'EAAIYLfie53cBOZBrMsQjT5ZC51knYl2ys2ZAGZBKCHASSyxnxiwnFc03Lt2ZAFgCNC4Uf5IuVGu7ZC7IvvgXd0CQNEIZCoZAp3fAztrcZBxVhhKbYswwUAdjBBSwZBTrmESUIOeKvZBWIhECDJou4l3uGt6FIBlR7pDNAibrFalM2nKJRX3aFT3IZCpnB6YcBCRrYu3P9kZCUvvonaA0lFDdAScwZD', N'266349363239226', N'Manhmauu', N'MANHMAUU', N'phamvanmanh1608@gmail.com', N'PHAMVANMANH1608@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEElNiAmmXkapyPyQTZq1Gw1vJpUwagtv/5+cQkwwXFeU1rOrGb4AUuYeWmZksvT5ng==', N'7NMMGZFHLL2QU2UYR6TSAOJEEMFLIIA3', N'24e3f068-fe15-4956-9369-c06eb063008f', N'0123456789', 0, 0, NULL, 1, 0)
INSERT [dbo].[Users] ([Id], [CreatedAt], [ImageURL], [FullName], [Sex], [Address], [TokenFacbook], [PageId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'c971072d-9cc8-44b8-ba4c-666ce652a346', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'https://res.cloudinary.com/loms/image/upload/v1745308519/tipluvoozjtgbb2etdx9.jpg', NULL, N'Male', N'hanoi', NULL, NULL, N'manhhh', N'MANHHH', N'mp16082345@gmail.com', N'MP16082345@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEC38QyTAynXQgw0l24fc9Ce0RuHGkf3ime9ttjXFhcq59uqR62YDrPBHahlyMAnPVA==', N'K3HHNYIBR5AIIHUE5CYPG7H7TDKGNGNV', N'941b9d01-2af9-4ea2-b8c4-636dea00b846', N'0222222222', 0, 0, NULL, 1, 0)
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (CONVERT([bit],(0))) FOR [StatusCheck]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_LiveStreamCustomers_LiveStreamCustomerID] FOREIGN KEY([LiveStreamCustomerID])
REFERENCES [dbo].[LiveStreamCustomers] ([LiveStreamCustomerId])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_LiveStreamCustomers_LiveStreamCustomerID]
GO
ALTER TABLE [dbo].[ListProducts]  WITH CHECK ADD  CONSTRAINT [FK_ListProducts_Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ListProducts] CHECK CONSTRAINT [FK_ListProducts_Users_UserID]
GO
ALTER TABLE [dbo].[LiveStreamCustomers]  WITH CHECK ADD  CONSTRAINT [FK_LiveStreamCustomers_Customers_CustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[LiveStreamCustomers] CHECK CONSTRAINT [FK_LiveStreamCustomers_Customers_CustomerID]
GO
ALTER TABLE [dbo].[LiveStreamCustomers]  WITH CHECK ADD  CONSTRAINT [FK_LiveStreamCustomers_LiveStreams_LivestreamID] FOREIGN KEY([LivestreamID])
REFERENCES [dbo].[LiveStreams] ([LivestreamID])
GO
ALTER TABLE [dbo].[LiveStreamCustomers] CHECK CONSTRAINT [FK_LiveStreamCustomers_LiveStreams_LivestreamID]
GO
ALTER TABLE [dbo].[LiveStreams]  WITH CHECK ADD  CONSTRAINT [FK_LiveStreams_ListProducts_ListProductID] FOREIGN KEY([ListProductID])
REFERENCES [dbo].[ListProducts] ([ListProductId])
GO
ALTER TABLE [dbo].[LiveStreams] CHECK CONSTRAINT [FK_LiveStreams_ListProducts_ListProductID]
GO
ALTER TABLE [dbo].[LiveStreams]  WITH CHECK ADD  CONSTRAINT [FK_LiveStreams_Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[LiveStreams] CHECK CONSTRAINT [FK_LiveStreams_Users_UserID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Comments_CommentID] FOREIGN KEY([CommentID])
REFERENCES [dbo].[Comments] ([CommentID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Comments_CommentID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Products_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Products_ProductID]
GO
ALTER TABLE [dbo].[ProductListProducts]  WITH CHECK ADD  CONSTRAINT [FK_ProductListProducts_ListProducts_ListProductID] FOREIGN KEY([ListProductID])
REFERENCES [dbo].[ListProducts] ([ListProductId])
GO
ALTER TABLE [dbo].[ProductListProducts] CHECK CONSTRAINT [FK_ProductListProducts_ListProducts_ListProductID]
GO
ALTER TABLE [dbo].[ProductListProducts]  WITH CHECK ADD  CONSTRAINT [FK_ProductListProducts_Products_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[ProductListProducts] CHECK CONSTRAINT [FK_ProductListProducts_Products_ProductID]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Users_UserID]
GO
ALTER TABLE [dbo].[RoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleClaims] CHECK CONSTRAINT [FK_RoleClaims_Roles_RoleId]
GO
ALTER TABLE [dbo].[UserClaims]  WITH CHECK ADD  CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserClaims] CHECK CONSTRAINT [FK_UserClaims_Users_UserId]
GO
ALTER TABLE [dbo].[UserLogins]  WITH CHECK ADD  CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserLogins] CHECK CONSTRAINT [FK_UserLogins_Users_UserId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles_RoleId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users_UserId]
GO
ALTER TABLE [dbo].[UserTokens]  WITH CHECK ADD  CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserTokens] CHECK CONSTRAINT [FK_UserTokens_Users_UserId]
GO