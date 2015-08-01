<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
	<w:body>
		<w:tbl>
			<w:tr>
				$ItemToDisplay.Properties.keys:{k |
				<w:tc>
					<w:p>
						<w:r>
							<w:rPr>
								<w:b w:val="on"/>
								<w:t>
									$ItemToDisplay.Properties.(k);format="XML"$
								</w:t>
							</w:rPr>
						</w:r>
					</w:p>
				</w:tc>
				}$
			</w:tr>
			$ItemToDisplay.SubNodes.keys:{k |$ItemToDisplay.SubNodes.(k):{y |
			$word_item(ItemToDisplayRec= y)$ }$}$
		</w:tbl>
	</w:body>
</w:document>
	
	
 