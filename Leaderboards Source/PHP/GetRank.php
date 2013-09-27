<?php
    //You need to fill in this information!
    $db = mysql_connect('SQLHOST', 'SQLUSER', 'SQLPASSWORD') or die('Failed to connect: ' . mysql_error()); 
        mysql_select_db('YOURDATABASE') or die('Failed to access database');
 
    $name = mysql_real_escape_string($_GET['name']); 
    
    //This is the polite version of our name
    $politestring = sanitize($name);
    
      //Here's our query to grab a rank.
      $query = "SELECT  uo.*, 
          (
          SELECT  COUNT(*)
          FROM    Score ui
          WHERE   (ui.score, -ui.ts) >= (uo.score, -uo.ts)
          ) AS rank
      FROM    Score uo
      WHERE   name = '$politestring';";
      $result = mysql_query($query) or die('Query failed: ' . mysql_error());
      
      //This is more elaborate than we need, considering we're only grabbing one rank, but you can modify it if needs be.
      $num_results = mysql_num_rows($result);  
      
      for($i = 0; $i < $num_results; $i++)
      {
           $row = mysql_fetch_array($result);
           echo $row['rank'] . "\n";
      }

/////////////////////////////////////////////////
// string sanitize functionality to avoid
// sql or html injection abuse and bad words
/////////////////////////////////////////////////
function no_naughty($string)
{
    $string = preg_replace('/shit/i', 'shoot', $string);
    $string = preg_replace('/fuck/i', 'fool', $string);
    $string = preg_replace('/asshole/i', 'animal', $string);
    $string = preg_replace('/bitches/i', 'dogs', $string);
    $string = preg_replace('/bitch/i', 'dog', $string);
    $string = preg_replace('/bastard/i', 'plastered', $string);
    $string = preg_replace('/nigger/i', 'newbie', $string);
    $string = preg_replace('/cunt/i', 'corn', $string);
    $string = preg_replace('/cock/i', 'rooster', $string);
    $string = preg_replace('/faggot/i', 'piglet', $string);

    $string = preg_replace('/suck/i', 'rock', $string);
    $string = preg_replace('/dick/i', 'deck', $string);
    $string = preg_replace('/crap/i', 'rap', $string);
    $string = preg_replace('/blows/i', 'shows', $string);

    // ie does not understand "&apos;" &#39; &rsquo;
    $string = preg_replace("/'/i", '&rsquo;', $string);
    $string = preg_replace('/%39/i', '&rsquo;', $string);
    $string = preg_replace('/&#039;/i', '&rsquo;', $string);
    $string = preg_replace('/&039;/i', '&rsquo;', $string);

    $string = preg_replace('/"/i', '&quot;', $string);
    $string = preg_replace('/%34/i', '&quot;', $string);
    $string = preg_replace('/&034;/i', '&quot;', $string);
    $string = preg_replace('/&#034;/i', '&quot;', $string);

    // these 3 letter words occur commonly in non-rude words...
    //$string = preg_replace('/fag', 'pig', $string);
    //$string = preg_replace('/ass', 'donkey', $string);
    //$string = preg_replace('/gay', 'happy', $string);
    return $string;
}

function my_utf8($string)
{
    return strtr($string,
      "/<>€µ¿¡¬ˆŸ‰«»Š ÀÃÕ‘¦­‹³²Œ¹÷ÿŽ¤Ððþý·’“”ÂÊÁËÈÍÎÏÌÓÔ•ÒÚÛÙž–¯˜™š¸›",
      "![]YuAAAAAAACEEEEIIIIDNOOOOOOUUUUYsaaaaaaaceeeeiiiionoooooouuuuyy");
}

function safe_typing($string)
{
    return preg_replace("/[^a-zA-Z0-9 \!\@\%\^\&\*\.\*\?\+\[\]\(\)\{\}\^\$\:\;\,\-\_\=]/", "", $string);
}

function sanitize($string)
{
    // make sure it isn't waaaaaaaay too long
    $MAX_LENGTH = 250; // bytes per chat or text message - fixme?
    $string = substr($string, 0, $MAX_LENGTH);
    $string = no_naughty($string);
    // breaks apos and quot: // $string = htmlentities($string,ENT_QUOTES);
    // useless since the above gets rid of quotes...
    //$string = str_replace("'","&rsquo;",$string);
    //$string = str_replace("\"","&rdquo;",$string);
    //$string = str_replace('#','&pound;',$string); // special case
    $string = my_utf8($string);
    $string = safe_typing($string);
    return trim($string);
}


?>