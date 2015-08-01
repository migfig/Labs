<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
	<title>$FirstItem.Name;format="toUpper"$ </title>
	<style type="text/css">
		table { border: 1px dotted gray; }        
        th { background: Orange; color: White; }
        td { border: 1px dotted gray; }        
        body { font: normal small-caps normal small Arial; }
	</style>
	</head>
	<body>
	
<Table border="1">
   <tr>
	$ItemToDisplay.Properties.keys:{k |
							<th>$ItemToDisplay.Properties.(k);format="XML"$ </th>
							}$
   </tr>
   $ItemToDisplay.SubNodes.keys:{k |$ItemToDisplay.SubNodes.(k):{y | 
					$html_item(ItemToDisplayRec= y)$ }$}$					
   
  </Table>
				
		Generated on $DateCreated;format="yyyy MMM dd"$
	</body>
</html>