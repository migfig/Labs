<itext author="Andrei Ignat" title="Collection">

<chapter numberdepth="0">
<newline />
<section numberdepth="0">
<table width="100%"  cellspacing="0" cellpadding="2" columns="$ItemToDisplay.Properties.keys.Count$" grayfill="0.90">
<row>
	$ItemToDisplay.Properties.keys:{k |	<cell><phrase font="Arial" size="12" style="bold">$ItemToDisplay.Properties.(k);format="XML"$</phrase></cell>}$
</row>
	$ItemToDisplay.SubNodes.keys:{k |$ItemToDisplay.SubNodes.(k):{y |$itextSharp_item(ItemToDisplayRec= y)$}$}$
</table>
</section>

</chapter>



</itext>
	
