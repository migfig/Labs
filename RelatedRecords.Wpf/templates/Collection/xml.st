<?xml version="1.0" encoding="utf-8" ?> 
<root>
	<dictionary>
		$ItemToDisplay.Properties.keys:{k |
		<$k$>$ItemToDisplay.Properties.(k);format="XML"$ </$k$>
		}$
	
</dictionary>
	
	$ItemToDisplay.SubNodes.keys:{k |$ItemToDisplay.SubNodes.(k):{y |
	$xml_item(ItemToDisplayRec= y)$ }$}$

</root>