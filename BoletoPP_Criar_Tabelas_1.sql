USE [BoletoPP_HOMOLOG]
GO

/****** Object:  Table [dbo].[PP_GUIA]    Script Date: 23/07/2024 16:38:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PP_GUIA](
	[Id_Guia] [int] IDENTITY(1,1) NOT NULL,
	[CodigoBarra] [varchar](44) NULL,
	[LinhaDigitavel] [varchar](60) NULL,
	[NossoNumero] [varchar](25) NULL,
	[NumDocumento] [varchar](14) NULL,
	[Valor_Guia] [float] NOT NULL,
	[Observacao] [ntext] NOT NULL,
	[Login] [varchar](20) NOT NULL,
	[PpID] [int] NOT NULL,
	[Data_Vencimento] [datetime] NOT NULL,
	[Data_Pagamento] [datetime] NULL,
	[Valor_Pago] [decimal](9, 2) NULL,
	[Banco_Pagamento] [smallint] NULL,
	[Data_Baixa] [datetime] NULL,
	[Data_Processamento] [datetime] NULL,
 CONSTRAINT [PK_PP_GUIA] PRIMARY KEY CLUSTERED 
(
	[Id_Guia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PP_GUIA]  WITH CHECK ADD  CONSTRAINT [FK_PP_GUIA_PP_PRECO_PUBLICO] FOREIGN KEY([PpID])
REFERENCES [dbo].[PP_PRECO_PUBLICO] ([ID])
GO

ALTER TABLE [dbo].[PP_GUIA] CHECK CONSTRAINT [FK_PP_GUIA_PP_PRECO_PUBLICO]
GO

ALTER TABLE [dbo].[PP_GUIA]  WITH CHECK ADD  CONSTRAINT [FK_PP_GUIA_PP_USUARIO] FOREIGN KEY([Login])
REFERENCES [dbo].[PP_USUARIO] ([Login])
GO

ALTER TABLE [dbo].[PP_GUIA] CHECK CONSTRAINT [FK_PP_GUIA_PP_USUARIO]
GO


USE [BoletoPP_HOMOLOG]
GO

/****** Object:  Table [dbo].[PP_PRECO_PUBLICO]    Script Date: 23/07/2024 16:38:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PP_PRECO_PUBLICO](
	[ID] [int] NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
	[Valor_UFM] [float] NOT NULL,
	[Cod_Orcamentario] [int] NOT NULL,
	[ID_TD] [int] NULL,
 CONSTRAINT [PK_PP_PRECO_PUBLICO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [BoletoPP_HOMOLOG]
GO

/****** Object:  Table [dbo].[PP_TIPO_DESCRICAO]    Script Date: 23/07/2024 16:38:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PP_TIPO_DESCRICAO](
	[ID_TD] [int] NOT NULL,
	[Descricao] [varchar](20) NULL,
 CONSTRAINT [PK_PP_TIPO_DESCRICAO] PRIMARY KEY CLUSTERED 
(
	[ID_TD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [BoletoPP_HOMOLOG]
GO

/****** Object:  Table [dbo].[PP_USUARIO]    Script Date: 23/07/2024 16:39:13 ******/
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

ALTER TABLE [dbo].[PP_USUARIO] ADD  CONSTRAINT [DF_PP_USUARIO_Alterar_Senha]  DEFAULT ((1)) FOR [Alterar_Senha]
GO

ALTER TABLE [dbo].[PP_USUARIO] ADD  CONSTRAINT [DF_PP_USUARIO_Administrador]  DEFAULT ((0)) FOR [Administrador]
GO

ALTER TABLE [dbo].[PP_USUARIO] ADD  CONSTRAINT [DF_PP_USUARIO_Ativo]  DEFAULT ((1)) FOR [Ativo]
GO


