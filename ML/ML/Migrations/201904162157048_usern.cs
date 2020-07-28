namespace ML.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usern : DbMigration
    {
        public override void Up()
        {
           AddColumn("dbo.AspNetUsers", "usern", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "usern");
        }
    }
}
