<html>
 <head>
  <title>PHP-Test</title>
 </head>
 <body>
  <?php

header('Access-Control-Allow-Origin: *');
	header('Access-Control-Allow-Methods: GET, POST, OPTIONS');
	header('Access-Control-Allow-Headers: Origin, Content-Type, Accept, Authorization, X-Request-With');
  header('Access-Control-Allow-Credentials: true');

   $userId = $_POST['id'];
    if(isset($_FILES['files']))
    {
      $total = count($_FILES['files']);
      $uploadError = false;

      for ( $i = 0; $i < $total; $i++)
      {
        $tmpFilePath = $_FILES['files']['tmp_name'][$i];
        
      echo $tmpFilePath;
        if ($tmpFilePath != "")
        {
            $directoryPath = "D:\\XAMP\htdocs\\turkicLanguages\\".$userId."\\";
            if(!file_exists($directoryPath))
            {
              mkdir($directoryPath);
            }
            $newFilePath = $directoryPath.$_FILES['files']['name'][$i];
            if (!move_uploaded_file($tmpFilePath, $newFilePath))
                $uploadError = true;
        }
      }
    }
   
   if ($uploadError)
       echo "Upload Error";
   else
       echo "Uploaded Successfully";
  ?>
 </body>
</html>