SELECT [platform].Id id, [platform].Name name
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
),
(
	SELECT [profile].Name name, [item].ProfileId, [item].PlatformId
	FROM [Profiles] [profile]
		INNER JOIN [ProfileItem] [item] ON [profile].Id = [item].ProfileId 
		RIGHT JOIN Platforms [platform] ON [item].PlatformId = [platform].Id
	WHERE [item].PlatformId = [platform].Id
	FOR XML AUTO, TYPE
)  
FROM Platforms [platform]	
FOR XML AUTO, TYPE, ROOT('configuration')