<Table class="className">
  <thead align="left">
    <tr>
      <td colspan="columnCount" style="font-weight:bold">tableName</td>
    </tr>
  </thead>
  
   <tr>
	$ItemToDisplay.Properties.keys:{k |
							<th>$ItemToDisplay.Properties.(k);format="XML"$ </th>
							}$
        
   </tr>
   $ItemToDisplay.SubNodes.keys:{k |$ItemToDisplay.SubNodes.(k):{y | 
					$html_item(ItemToDisplayRec= y)$ }$}$					
   
  </Table>
