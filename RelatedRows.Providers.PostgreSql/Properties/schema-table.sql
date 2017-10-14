SELECT XMLELEMENT(name "schema",
	XMLAGG(xml_tables)
	)                  
FROM 
	(SELECT 
		XMLELEMENT(name "Table", 
    		XMLATTRIBUTES(
				concat('"', catalog, '"') as catalog,
				concat('"', schemaName, '"') as "schemaName",
				concat('"', name, '"') as name,
				'0' as rows
			),
			XMLAGG(xml_columns)
		) as xml_tables
	FROM (
		SELECT 
    		t.table_catalog as catalog,
    		t.table_schema as schemaName,
    		t.table_name as name,
    		XMLELEMENT(name "Column",
        		XMLATTRIBUTES(
					concat('"', c.column_name, '"') as name,
					c.ORDINAL_POSITION as ordinal,
					CASE c.DATA_TYPE 
						WHEN '"char"' THEN 'char'
						WHEN 'abstime' THEN 'timestamp'
						WHEN 'anyarray' THEN 'binary'
						WHEN 'ARRAY' THEN 'binary'
						WHEN 'boolean' THEN 'bool'
						WHEN 'bytea' THEN 'binary'
						WHEN 'character' THEN 'char'
						WHEN 'character varying' THEN 'string'
						WHEN 'double precision' THEN 'long'
						WHEN 'inet' THEN 'string'
						WHEN 'integer' THEN 'int'
						WHEN 'interval' THEN 'string'
						WHEN 'json' THEN 'string'
						WHEN 'jsonb' THEN 'string'
						WHEN 'name' THEN 'string'
						WHEN 'numeric' THEN 'float'
						WHEN 'oid' THEN 'string'
						WHEN 'pg_lsn' THEN 'string'
						WHEN 'pg_node_tree' THEN 'string'
						WHEN 'regproc' THEN 'string'
						WHEN 'regprocedure' THEN 'string'
						WHEN 'timestamp with time zone' THEN 'timestamp'
						WHEN 'timestamp without time zone' THEN 'timestamp'
						WHEN 'xid' THEN 'string'
						ELSE c.DATA_TYPE
					END AS "DbType", 
					c.CHARACTER_MAXIMUM_LENGTH AS "maxLen", 
					NULLIF(c.NUMERIC_PRECISION, c.DATETIME_PRECISION) AS precision, 
					CASE c.IS_NULLABLE WHEN 'YES' THEN 1 ELSE 0 END AS "isNullable", 
					c.COLUMN_DEFAULT as "defaultValue",			
					(
						select case when count(*) >= 1 then 1 else 0 end
						from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
						where ccu.TABLE_NAME = t.TABLE_NAME and ccu.COLUMN_NAME = c.COLUMN_NAME and left(ccu.CONSTRAINT_NAME, 2) = 'PK'
					) AS "isPrimaryKey",
					(
						select case when count(*) >= 1 then 1 else 0 end
						FROM information_schema.table_constraints AS tc
						JOIN information_schema.key_column_usage AS kcu
    						ON tc.constraint_name = kcu.constraint_name
    					JOIN information_schema.constraint_column_usage AS ccu
    						ON ccu.constraint_name = tc.constraint_name
						WHERE tc.table_name = t.table_name and tc.constraint_type = 'FOREIGN KEY' and kcu.column_name = c.column_name
					) AS "isForeignKey"	
				),
				(
					SELECT XMLELEMENT(name "Relationship",
                		XMLATTRIBUTES(
							'"' || tc.constraint_schema || '"' AS "toSchemaName",
							'"' || ccu.table_name || '"' AS "toTable",
    						'"' || ccu.column_name || '"' AS "toColumn"
						)
					) 
					FROM information_schema.table_constraints AS tc
						JOIN information_schema.key_column_usage AS kcu
    						ON tc.constraint_name = kcu.constraint_name
    					JOIN information_schema.constraint_column_usage AS ccu
    						ON ccu.constraint_name = tc.constraint_name
					WHERE tc.table_name = t.table_name and tc.constraint_type = 'FOREIGN KEY' and kcu.column_name = c.column_name
					LIMIT 1
				)
			) as xml_columns
		FROM information_schema.TABLES t
			JOIN information_schema.COLUMNS c 
				ON t.table_name = c.table_name			
		WHERE 
    		t.TABLE_NAME = '@TableName'
		ORDER BY
    		t.table_catalog, t.table_schema, t.table_name, c.ordinal_position
		) Q
	GROUP BY catalog, schemaName, name
	) PQ
   		
