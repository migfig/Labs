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
			FOR XML AUTO, TYPE
		)
		FROM [Areas] [area]
		WHERE [area].KnowledgeAreaId = [knowledgeArea].Id
		FOR XML AUTO, TYPE
	)
	FROM [KnowledgeAreas] [knowledgeArea]
	WHERE [knowledgeArea].PlatformId = [platform].Id
	FOR XML AUTO, TYPE
) 
FROM Platforms [platform]
FOR XML AUTO, TYPE, ROOT('configuration')