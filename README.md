# ActorModelExample

Ok, you have selected the AKKA.net actor model.
That is stupid actor model or maybe it is fine, no it a great actor model find out yourself

This workshop has prepared some stuff so you can use AKKA.net.
We need a entry point to access our actors. The application is build with a background service were the AKKA.net system runs. Fot this workshop it is good enough. The background service is called AkkaService.
So you don't need to worry about that and can focus on the actors.

We have one entry point actor that is called VenueActor. The AKKA.net model is build as tree with parent en childeren. So the VenueActor is for us the root actor.

You need the build child actors and messages to interact and create a booking

----

This is an example booking application for Ticketmain.
The service is build for senior people so there are only seats. 

In the main branch is just the basic application.
So every thing works.

But for the challenges we have created to branches.
* branch __orleans__: For developers who want's to learn Orleans the actor model of Microsoft
* branch __akka.net__: For developers who want's to learn AKKA.net the actor model of Petabridge 

Both branches are not complete. You need to define your own actors/grain.
Good luck!

Statler and Waldorf  