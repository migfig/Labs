SELECT  '[' + TABLE_CATALOG + ']' AS catalog, '[' + TABLE_SCHEMA + ']' AS schemaName, '[' + TABLE_NAME + ']' AS name,
(
        SELECT 
			'[' + [Column].COLUMN_NAME + ']' AS name, 
			[Column].ORDINAL_POSITION as ordinal,
			[Column].DATA_TYPE AS DbType, 
			[Column].CHARACTER_MAXIMUM_LENGTH AS maxLen, 
			ISNULL([Column].NUMERIC_PRECISION, [Column].DATETIME_PRECISION) AS [precision], 
			CASE [Column].IS_NULLABLE WHEN 'YES' THEN 1 ELSE 0 END AS isNullable, 
			[Column].COLUMN_DEFAULT as defaultValue,
			'isPrimaryKey' =
			(
				select case when count(*) >= 1 then 1 else 0 end
				from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
				where ccu.TABLE_NAME = [Table].TABLE_NAME and ccu.COLUMN_NAME = [Column].COLUMN_NAME and left(ccu.CONSTRAINT_NAME, 2) = 'PK'
	        ),
		    'isForeignKey' =
			(
				select case when count(*) >= 1 then 1 else 0 end
				from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
				where ccu.TABLE_NAME = [Table].TABLE_NAME and ccu.COLUMN_NAME = [Column].COLUMN_NAME and left(ccu.CONSTRAINT_NAME, 2) = 'FK'
	        ),
			'isIdentity' = 
			(
				select sc.is_identity
				from sys.tables st 
					inner join sys.columns sc on sc.object_id = st.object_id 
				where st.name = [Table].TABLE_NAME and sc.name = [Column].COLUMN_NAME
			),
			(
				SELECT DISTINCT '[' + ccupk.TABLE_NAME + ']' as toTable, '[' + ccupk.COLUMN_NAME + ']' as toColumn
				FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE [Relationship] 
					INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccupk ON [Relationship].COLUMN_NAME = ccupk.COLUMN_NAME
				WHERE [Relationship].TABLE_NAME = [Table].TABLE_NAME and [Relationship].COLUMN_NAME = [Column].COLUMN_NAME 
					and left([Relationship].CONSTRAINT_NAME, 2) = 'FK' and ccupk.TABLE_NAME <> [Table].TABLE_NAME
				FOR XML AUTO, TYPE
			)
        
        FROM information_schema.columns AS [Column]
        WHERE [Column].TABLE_NAME = [Table].TABLE_NAME
        ORDER BY [Column].ORDINAL_POSITION
        FOR XML AUTO, TYPE
)
FROM information_schema.TABLES AS [Table]
WHERE 
	TABLE_TYPE = 'BASE TABLE' 
	AND TABLE_NAME = '@tableName'
ORDER BY 1, 2, 3
FOR XML AUTO, TYPE, ROOT('schema')