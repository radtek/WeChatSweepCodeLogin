<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Console.aspx.cs" Inherits="Console_Console" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>控制台</title>    
    <link href="../Manager/css/base.css" rel="stylesheet" />
    <link href="../js/bootstrap/css/bootstrap.min.css" rel="stylesheet" />

    <link href="../js/bootstrap-table/bootstrap-table.css" rel="stylesheet" />
    <link href="../js/bootstrap-table/master/examples.css" rel="stylesheet" />
      <script src="../js/jquery/jquery-1.12.3.js"></script>
     <script src="../js/bootstrap/js/bootstrap.min.js"></script>
     <script src="../js/app/app.bootstrap.dialog.js"></script>
  
    <script src="../js/bootstrap-table/master/ga.js"></script>


  
    <script src="../js/app.js"></script>
    <script src="../js/console/console.js" type="text/javascript"></script>
    <style>
        body {
            font-family: "微软雅黑";
            margin: 0;
            padding: 0;
            height: 100%;
            max-height: 100%;
            overflow: hidden;
        }

        .console {
            position: absolute;
            top: 0;
            bottom: 0;
            width: 100%;
            height: 100%;
            background: #CCCCCC;
            opacity: 0.8;
            border-radius: 5px;
        }

            .console:hover {
                opacity: 1;
            }

        .consolebox {
            position: absolute;
            top: 37px;
            bottom: 10px;
            left: 10px;
            right: 10px;
            width: 97.5%;
            resize: none;
            background: #4a4a4a;
            color: #FFFFCC;
            font-size: 14px;
            padding: 5px;
            border-radius: 5px;
            box-shadow: inset 4px 4px 2px #404040;
            border: 1px solid #404040;
            font-weight: bold;
            outline: medium;
        }
    </style>

</head>
<body>
    <div class="console" id="console">
        <textarea class="consolebox" id="consolebox" rows="50" cols="100" onkeydown="keyup(this);"></textarea>
    </div>
    
    <!-- 弹出框 -->
    <div class="modal fade" id="modal-6">
        <div class="modal-dialog">
            <div class="modal-content" style="width:700px; height:600px; overflow:auto">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title"><i class="fa-ellipsis-v marginr10"></i>数据查询结果</h4>
                </div>
                <div class="modal-body">
                    <table class="a_table" id="table">
                    </table>
                </div>
            </div>
        </div>
    </div>
    <!-- 弹出框 end -->
</body>
</html>
<script src="../js/app.js"></script>
<script src="../js/app/app.bootstrap.table.js"></script>

