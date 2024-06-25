
/****** Object:  Table [dbo].[PP_GUIA]    Script Date: 21/06/2024 16:06:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PP_CARNE](
	[Codigo] [varchar] (10) NOT NULL,
	[Parcela] [int] NOT NULL,
	[Valor_UFM] [float] NOT NULL,
	[Valor_Real] [float] NOT NULL,
	[Observacao][ntext] NOT NULL,
	[Login][varchar](20) NOT NULL,
	[Data_Vencimento] [datetime] NOT NULL,
	[Data_Pagamento] [datetime] NULL,
 CONSTRAINT [PK_PP_GUIA] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PP_CARNE] ADD  CONSTRAINT [DF_PP_CARNE_Parcela]  DEFAULT ((99)) FOR [Parcela]
GO


/****** Object:  Table [dbo].[PP_PRECO_PUPLICO]    Script Date: 21/06/2024 16:06:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PP_PRECO_PUBLICO](
	[ID] [int] NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
	[Valor_UFM] [float] NOT NULL,
	[Cod_Orcamentario] [int] NOT NULL,
 CONSTRAINT [PK_PP_PRECO_PUBLICO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[PP_USUARIO]    Script Date: 21/06/2024 16:06:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PP_USUARIO](
	[Login] [varchar](20) NOT NULL,
	[Nome] [varchar](50) NULL,
	[Matricula] [int] NULL,
	[Secretaria] [varchar](30) NULL,
	[Senha] [varchar](20) NULL,
	[Alterar_Senha] [bit] NULL,
	[Administrador] [bit] NOT NULL,
	[Ativo] [bit] NOT NULL,
 CONSTRAINT [PK_PP_USUARIO] PRIMARY KEY CLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PP_USUARIO] ADD  CONSTRAINT [DF_PP_USUARIO_Senha]  DEFAULT ((1234)) FOR [Senha]
GO

ALTER TABLE [dbo].[PP_USUARIO] ADD  CONSTRAINT [DF_PP_USUARIO_Alterar_Senha]  DEFAULT (1) FOR [Alterar_Senha]
GO

ALTER TABLE [dbo].[PP_USUARIO] ADD  CONSTRAINT [DF_PP_USUARIO_Administrador]  DEFAULT (0) FOR [Administrador]
GO

ALTER TABLE [dbo].[PP_USUARIO] ADD  CONSTRAINT [DF_PP_USUARIO_Ativo]  DEFAULT (1) FOR [Ativo]
GO


