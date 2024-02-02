INCLUDE globals.ink


{(isCompleted_Julia == false && isStarted_Julia == false) : -> questNotStarted}

{(isCompleted_Julia == false && isStarted_Julia == true) : -> questStarted}

{(isCompleted_Julia == true && isStarted_Julia == true) : -> questCompleted}

=== questNotStarted ===
Te voilà bloqué avec moi... #speaker:Julia
Si tu veux pouvoir repartir de l'autre côté, il va falloir me rendre service.
J'ai besoin d'une épée en or, la plus tranchante de toutes.
Si tu me l'apportes, alors je pourrais t'aider à repartir de l'autre côté.
+ [D'accord]
    ~ startQuest(2)
    ~ isStarted_Julia = true
    Très bon choix !
    -> DONE
+ [Non merci]
    Tu resteras bloqué ici.
    -> DONE 

=== questStarted === 
As-tu trouvé l'épée dont je t'ai parlé ? #speaker:Julia
+ [Oui]
    ~ canBeCompleted_Julia = checkCanCompleteQuest(2) 
    {canBeCompleted_Julia == false : -> StartedCantBeCompleted | -> StartedCanBeCompleted}
    -> DONE
+ [Non]
    Me voila très triste...
    -> DONE 

=== StartedCantBeCompleted ===
Tu dis n'importe quoi.
-> DONE

=== StartedCanBeCompleted ===
Elle est si tranchante ! Tiens, prends cette clé en retour, elle te servira surement. 
~ completeQuest(2)
~ isCompleted_Julia = true
-> DONE



=== questCompleted ===
Merci pour l'épée, bon voyage ! #speaker:Julia
-> DONE


