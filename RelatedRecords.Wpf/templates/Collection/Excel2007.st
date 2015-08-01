<?xml version="1.0" encoding="UTF-8" standalone="yes" ?>
<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
	<sheetData>
		<row>
			$ItemToDisplay.Properties.keys:{k |
			<c t="inlineStr">
				<is>
					<t>$ItemToDisplay.Properties.(k);format="XML"$ </t>
				</is>
			</c>
			}$

		</row>
		$ItemToDisplay.SubNodes.keys:{k |$ItemToDisplay.SubNodes.(k):{y |
		$excel2007_item(ItemToDisplayRec= y)$ }$}$
	</sheetData>
</worksheet>



