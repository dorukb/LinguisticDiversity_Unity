<html>
 <head>
  <title>PHP-Test</title>
 </head>
 <body>
  <?php
   $total = count($_FILES['files']['name']);
   $uploadError = false;
   echo $total;
   
   for ( $i = 0; $i < $total; $i++)
   {
    echo $_FILES['files']['error'][$i];
    echo "\t";
    echo $_FILES['files']['name'][$i];
    echo $_FILES['files']['size'][$i];
    echo $_FILES['files']['type'][$i];

     $tmpFilePath = $_FILES['files']['tmp_name'][$i];
    
   echo $tmpFilePath;
     if ($tmpFilePath != "")
     {
         $newFilePath = "D:\\XAMP\htdocs\\turkicLanguages\\".$_FILES['files']['name'][$i];
         if (!move_uploaded_file($tmpFilePath, $newFilePath))
             $uploadError = true;
     }
   }
   if ($uploadError)
       echo "Upload Error";
   else
       echo "Uploaded Successfully";
  ?>
 </body>
</html>