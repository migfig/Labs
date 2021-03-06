USE [Interviewer]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetInterviewerData]    Script Date: 2/26/2016 6:16:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[usp_GetInterviewerData]
	@Platform nvarchar(255) = NULL,
	@KnowledgeArea nvarchar(150) = NULL,
	@Area nvarchar(150) = NULL,
	@Question nvarchar(max) = NULL
AS
BEGIN
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [platform].Name name 
	,(
		SELECT [knowledgeArea].Name name
		,(
			SELECT [area].Name name
			,(
				SELECT [question].Weight [weight],
					[question].Level [level],
					[question].Value [value]
				FROM [Questions] [question]
				WHERE [question].AreaId = [area].Id
					AND UPPER([question].Value) LIKE IIF(@Question IS NULL, '%', '%' + UPPER(@Question) + '%')
				FOR XML AUTO, TYPE
			)
			FROM [Areas] [area]
			WHERE [area].KnowledgeAreaId = [knowledgeArea].Id
				AND UPPER([area].Name) LIKE IIF(@Area IS NULL, '%', '%' + UPPER(@Area) + '%')
			FOR XML AUTO, TYPE
		)
		FROM [KnowledgeAreas] [knowledgeArea]
		WHERE [knowledgeArea].PlatformId = [platform].Id
			AND UPPER([knowledgeArea].Name) LIKE IIF(@KnowledgeArea IS NULL, '%', '%' + UPPER(@KnowledgeArea) + '%')
		FOR XML AUTO, TYPE
	) 
	FROM Platforms [platform]
	WHERE UPPER([platform].Name) LIKE IIF(@Platform IS NULL, N'%', '%' + UPPER(@Platform) + '%')
	FOR XML AUTO, TYPE, ROOT('configuration')
END
