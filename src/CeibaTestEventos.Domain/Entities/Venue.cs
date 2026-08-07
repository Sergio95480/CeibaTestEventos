using CeibaTestEventos.Domain.Common;

namespace CeibaTestEventos.Domain.Entities;

public sealed class Venue : Entity
{
    public string Nombre { get; private set; }

    public string Ciudad { get; private set; }

    public int Capacidad { get; private set; }


    private Venue()
    {
        Nombre = string.Empty;
        Ciudad = string.Empty;
    }


    public Venue(
        string nombre,
        string ciudad,
        int capacidad)
    {
        Validate(nombre, ciudad, capacidad);

        Nombre = nombre.Trim();
        Ciudad = ciudad.Trim();
        Capacidad = capacidad;
    }


    public void ActualizarDatos(
        string nombre,
        string ciudad,
        int capacidad)
    {
        Validate(nombre, ciudad, capacidad);

        Nombre = nombre.Trim();
        Ciudad = ciudad.Trim();
        Capacidad = capacidad;
    }


    private static void Validate(
        string nombre,
        string ciudad,
        int capacidad)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new DomainException(
                "El nombre del venue es obligatorio.");
        }


        if (string.IsNullOrWhiteSpace(ciudad))
        {
            throw new DomainException(
                "La ciudad del venue es obligatoria.");
        }


        if (capacidad <= 0)
        {
            throw new DomainException(
                "La capacidad del venue debe ser mayor que cero.");
        }
    }
}