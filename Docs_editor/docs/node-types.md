
MoonBehavior Comes with a lot of default build-in nodes that you can use.

In this page will explain the functionality of basic nodes.

#Sequencer 

<div class="mermaid">
graph LR
    A[Sequencer]
    A --> B[Task1]
    A --> C[Task2]
    A --> D[Task N]
    
    style B fill:#54ff00
    style C fill:#ffd400
    style A fill:#ffd400
</div>

Sequencer node executes sequencially every task by order from minor to mayor.

Returns SUCCESS when all tasks are successed, FAIL when any task fails, otherwise it will return RUNNING.

It's good for sequencial logic.

#Selector 

<div class="mermaid">
graph LR
    A[Selector]
    A --> B[Task1]
    A --> C[Task2]
    style B fill:#ed4444
    style C fill:#ffd400
    style A fill:#ffd400
</div>

Selector Executes task sequencially (like sequencer) but it will not execute to the next task until the current task fails.

Returns SUCCESS if the current task are successed , RUNNING if the current task are running , and FAILURE if all the childs tasks fails or it has no childs.

**Options**

>**Randomize:** If enabled it will sort and executes the childs Ramdomly.

It's good for IF-ELSE Sequences

#Random Selector 

<div class="mermaid">
graph LR
    A[Random Selector]
    A --> B[Task1]
    A --> C[Task2]
    A --> D[Task3]
    A --> E[Task N]
    style D fill:#ffd400
    style A fill:#ffd400
</div>


Random selector

Works like selector but select it's childs based on the priority of every child node, mayor priority means more selection probability. 

#Repeater

<div class="mermaid">
graph LR
    A[Repeater]
    A --> B[Task1]
    style B fill:#ffd400
    style A fill:#ffd400
</div>

Repeats a child Task every tick.

**Options**

>**Repeat until:** Repeat mode: SUCCESS; Repeats until the child node returns Success, FAILURE; Repeats until the child node retunrs Failure, 
FOREVER; Repeats forever.

Returns Success When the repeat condition fails, otherwise returns running

it's good for continuos executions.

#Iterator 

<div class="mermaid">
graph LR
    A[Iterator]
    A --> B[Task1]
    style B fill:#ffd400
    style A fill:#ffd400
</div>

Repeats a child task every tick (like Repeater) but with a limited repeat count.


**Options**

>**Repeat count:** Maximun child repetitions.

Returns SUCCESS when repetitions are finished, otherwise returns RUNNING.

#Parallel


<div class="mermaid">
graph LR
    A[Parallel]
    A --> B[Task1]
    A --> C[Task2]
    A --> D[Task3]
    A --> E[Task N]
    style A fill:#ffd400
    style B fill:#ffd400
    style C fill:#ffd400
    style D fill:#ffd400
    style E fill:#ffd400
</div>

Executes all childs simultaneously every tick

Returns RUNNING.

#Inverter


<div class="mermaid">
graph LR
    A[Inverter]
    A --> B[Task1]
    style B fill:#54ff00
    style A fill:#ed4444
</div>

Like ! Operator Returns the invert result of it's child Task.

SUCCESS becomes FAILURE

FAILURE becomes SUCCESS.