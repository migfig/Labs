SELECT '' AS '@catalog', '[' + sch.name +']' as '@schemaName', '['+[Query].Name+']' AS '@name'
,RTRIM([Query].[Type]) as '@type'
,(   
	SELECT [Parameter].[Name] AS 'name'
	     ,'type' =	(     
			SELECT TOP 1 types.[Name]      
			FROM sys.systypes types     
			WHERE types.[xtype] = [Parameter].[system_type_id]
		)
		,'length' = (
			SELECT TOP 1 types.[Length]      
			FROM sys.systypes types     
			WHERE types.[xtype] = [Parameter].[system_type_id]
		)
		,[Parameter].[default_value] as defaultValue   
	FROM sys.all_parameters as [Parameter]     
			INNER JOIN sys.all_sql_modules [mods] ON [mods].[object_id] = [Parameter].[object_id]    
	WHERE [Parameter].[object_id] = [Query].[object_id]   
	FOR XML AUTO, TYPE  
)
,'Text' =  
(
   SELECT '<![CDATA[' + [definition] + ']]>' AS [Text]   
   FROM sys.all_sql_modules    
   WHERE [object_id] = [Query].[object_id]  
) 
FROM sys.all_objects AS [Query] INNER JOIN sys.schemas sch ON [Query].schema_id = sch.schema_id
WHERE [Query].[Type] IN ('P', 'FN', 'IF', 'AF', 'TF') 
	AND [Query].[schema_id] NOT IN (SELECT [schema_id] FROM sys.schemas where [Name] IN ('sys')) 
FOR XML PATH('Query'), TYPE, ROOT('schema')