using Orm.Console.Models;
using Orm.Core;
using Orm.Core.Migration;

namespace Orm.Console.Migrations;


internal sealed class Migration_001_CreateInitialSchema : IMigration
{
    public string Id => "001_CreateInitialSchema";

    public void Up(OrmContext db)
    {
        db.CreateTable<Doctor>();
        db.CreateTable<Patient>();
        db.CreateTable<Medication>();
        db.CreateTable<DiseaseHistory>();
        db.CreateTable<Appointment>();
        db.CreateTable<MedicationPrescription>();
    }
}