INCLUDE globals.ink



{(isCompleted_Francesco == false && isStarted_Francesco == false) : -> questNotStarted}

{(isCompleted_Francesco == false && isStarted_Francesco == true) : -> questStarted}

{(isCompleted_Francesco == true && isStarted_Francesco == true) : -> questCompleted}

=== questNotStarted ===
Coucou petit chat, tu m'as l'air bien perdu... #speaker:Francesco
Tu tombes bien, on a besoin de toi !
La forêt est en grand danger, et pour la sauver il faut absolument que tu me trouves un Sceptre de Saphir.
La récompense vaut le coup, crois moi.
+ [J'accepte]
    A très vite alors petit chat !
    ~ isStarted_Francesco = true
    ~ startQuest(0)
    
    -> DONE
+ [Je n'ai pas que ça à faire]
    Tu passes à côté d'une offre exceptionnelle...
    -> DONE 

=== questStarted ===
Ah, encore là ? #speaker:Francesco
As-tu trouvé le Sceptre de Saphir dont je t'ai parlé ?
+ [Oui]
    ~ canBeCompleted_Francesco = checkCanCompleteQuest(0) 
    {canBeCompleted_Francesco == false : -> StartedCantBeCompleted | -> StartedCanBeCompleted}
    -> DONE
+ [Non]
    Comment oses-tu...
    -> DONE 

=== StartedCantBeCompleted ===
Tu m'as menti, tu ne l'as pas. Je suis très déçu.
-> DONE

=== StartedCanBeCompleted ===
Oh mon dieu un Sceptre de Saphir ! Je te ramène chez toi, suis moi !
~ completeQuest(0)
~ isCompleted_Francesco = true
-> DONE



=== questCompleted ===
Merci infiniment d'avoir ramené mon Sceptre de Saphir, à la prochaine petit chat ! #speaker:Francesco
-> DONE


