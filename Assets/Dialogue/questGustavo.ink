INCLUDE globals.ink

{(isCompleted_Gustavo == false && isStarted_Gustavo == false) : -> questNotStarted1}

{(isCompleted_Gustavo == false && isStarted_Gustavo == true) : -> questStarted}

{(isCompleted_Gustavo == true && isStarted_Gustavo == true) : -> questCompleted}

=== questNotStarted1 ===
Coucou toi ! Content de te voir, j'ai une surprise pour toi. #speaker:Gustavo
As-tu croisé une table de craft ?
+ [Oui]
    Oh super ! Alors tu vas pouvoir m'aider.
    -> questNotStarted
    -> DONE
+ [Non]
    C'est un gros carré avec lequel tu peux créer des outils, des potions...
    Il y en a plusieurs dans le monde alors tu en croiseras forcément une, regarde autour de toi !
    -> questNotStarted
    -> DONE


=== questNotStarted ===
Voila, hop, je t'apprends à utiliser toutes les tables de craft que tu croiseras !
Mais je ne fais pas ça gratuitement...
J'ai besoin tu me promettes d'en prendre soin et de me ramener un marteau, le mien vient de casser !
+ [J'accepte]
    ~ startQuest(1)
    ~ isStarted_Gustavo = true
    Je te fais confiance petit, à plus tard !
    -> DONE
+ [Non merci]
    Mince... Au revoir petit chat.
    -> DONE 

=== questStarted ===
Coucou ! #speaker:Gustavo
As-tu un marteau pour moi ?
+ [Oui]
    ~ canBeCompleted_Gustavo = checkCanCompleteQuest(1) 
    {canBeCompleted_Gustavo == false : -> StartedCantBeCompleted | -> StartedCanBeCompleted}
    -> DONE
+ [Non]
    Je t'attends alors !
    -> DONE 

=== StartedCantBeCompleted ===
C'est complètement faux. Je vais garder mon marteau cassé.
-> DONE

=== StartedCanBeCompleted ===
Tu me sauves la vie ! 
Merci pour le marteau tout neuf, prends cette recette en cadeau, elle ne me sert pas !
~ completeQuest(1)
~ isCompleted_Gustavo = true
-> DONE



=== questCompleted ===
Merci beaucoup pour le marteau, j'espère que la recette que je t'ai donnée a pu t'être utile ! #speaker:Gustavo
-> DONE

