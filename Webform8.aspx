
<%@ Page Language="C#" AutoEventWireup="true"   CodeBehind="WebForm8.aspx.cs" Inherits="MiidWeb.WebForm8"  MasterPageFile="~/Site1.Master"  %>

     <asp:Content ID="Content1" ContentPlaceHolderID ="ContentPlaceHolder1" runat="server" >
        <form id="form2" runat="server">
            <div class="col-md-9">

				  <div class="row">
                     <div class="col-md-12">
						  <div class="content-box-header">					
							  <div class="panel-title">Payfast Errors</div>
						     
                        </div>
						 <div class="content-box-large box-with-header">
							  <div class="table-responsive" style="overflow-x:auto;">
								  <div id="dvData2">
			    					   <asp:GridView ID="GridViewtickets" runat="server" AutoGenerateColumns="true"  AllowPaging="true" OnPageIndexChanging = "OnPaging">  
									   </asp:GridView>
								  </div>
							  </div>
						 </div>
				    </div>
				</div>


           




<!--download first table csv script-->
<script language="javascript" type="text/javascript">
	$(document).ready(function() {

  function exportTableToCSV($table, filename) {

    var $rows = $table.find('tr:has(td)'),

      // Temporary delimiter characters unlikely to be typed by keyboard
      // This is to avoid accidentally splitting the actual contents
      tmpColDelim = String.fromCharCode(11), // vertical tab character
      tmpRowDelim = String.fromCharCode(0), // null character

      // actual delimiter characters for CSV format
      colDelim = '","',
      rowDelim = '"\r\n"',

      // Grab text from table into CSV formatted string
      csv = '"' + $rows.map(function(i, row) {
        var $row = $(row),
          $cols = $row.find('td');

        return $cols.map(function(j, col) {
          var $col = $(col),
            text = $col.text();

          return text.replace(/"/g, '""'); // escape double quotes

        }).get().join(tmpColDelim);

      }).get().join(tmpRowDelim)
      .split(tmpRowDelim).join(rowDelim)
      .split(tmpColDelim).join(colDelim) + '"';

    // Deliberate 'false', see comment below
    if (false && window.navigator.msSaveBlob) {

      var blob = new Blob([decodeURIComponent(csv)], {
        type: 'text/csv;charset=utf8'
      });

      // Crashes in IE 10, IE 11 and Microsoft Edge
      // See MS Edge Issue #10396033
      // Hence, the deliberate 'false'
      // This is here just for completeness
      // Remove the 'false' at your own risk
      window.navigator.msSaveBlob(blob, filename);

    } else if (window.Blob && window.URL) {
      // HTML5 Blob        
      var blob = new Blob([csv], {
        type: 'text/csv;charset=utf-8'
      });
      var csvUrl = URL.createObjectURL(blob);

      $(this)
        .attr({
          'download': filename,
          'href': csvUrl
        });
    } else {
      // Data URI
      var csvData = 'data:application/csv;charset=utf-8,' + encodeURIComponent(csv);

      $(this)
        .attr({
          'download': filename,
          'href': csvData,
          'target': '_blank'
        });
    }
  }

  // This must be a hyperlink
  $(".export").on('click', function(event) {
    // CSV
    var args = [$('#dvData>div>table'), 'export.csv'];

    exportTableToCSV.apply(this, args);

    // If CSV, don't do event.preventDefault() or return false
    // We actually need this to be a typical hyperlink
  });
});

</script>			
<!-- download csv script-->


    
 </form>


    
    </asp:Content>
