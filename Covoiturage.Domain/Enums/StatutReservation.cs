namespace Covoiturage.Domain.Enums;

public enum StatutReservation
{
    EnAttente = 0,            // Réservé par le passager, en attente de l'appel/validation du conducteur
    Confirmee = 1,            // Validé par le conducteur -> places décrémentées
    Refusee = 2,              // Refusé par le conducteur
    DemandeAnnulation = 3,    // Le passager demande l'annulation d'un trajet déjà confirmé
    Annulee = 4               // Trajet annulé et places restituées au trajet
}